using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public class TileLayer
    {
        #region Variables

        //Drawing size of the tiles.
        static int tileWidth = 64;
        static int tileHeight = 64;
    
        //A List of the textures being used in the layer
        List<Texture2D> tileTextures = new List<Texture2D>();

        //The array of the layer map
        //Values of -1 will be treated as blanks spaces and values that are indexes of the tileTextures will draw the repective texture
        int[,] map;

        //Transparency 
        float alpha = 1f;

        #endregion //Variables

        #region Properties

        //returns/sets tile width
        public static int TileWidth
        {
            get { return tileWidth; }
            set
            {
                //restricts tile width to (20,100)
                tileWidth = (int)MathHelper.Clamp(value, 20f, 100f);
            }
        }

        //returns/sets tile height
        public static int TileHeight
        {
            get { return tileHeight; }
            set
            {
                //restricts tile height to (20,100)
                tileHeight = (int)MathHelper.Clamp(value, 20f, 100f);
            }
        }

        //return/sets alpha (Transparency level)
        public float Alpha
        {
            get { return alpha; }
            set
            {
                //restricts alpha to (0,1)
                alpha = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        //returns the width of the map in pixels
        public int WidthInPixels
        {
            get { return Width * tileWidth; }
        }

        //returns the height of the map in pixels
        public int HeightInPixels
        {
            get { return Height * tileHeight; }
        }

        //returns the width of the map (# of tiles wide)
        public int Width
        {
            get { return map.GetLength(1); }
        }

        //returns the height of the map (# of tiles high)
        public int Height
        {
            get { return map.GetLength(0); }
        }

        #endregion //Properties

        #region Constructors

        //Takes in a width and height of a map layer
        public TileLayer(int width, int height)
        {
            map = new int[height, width];

            //Sets values of the map to -1 (all blank spaces)
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[y, x] = -1;
        }

        //Takes in an existing map layer
        public TileLayer(int[,] existingMap)
        {
            //Creates a cope of the map passed in
            map = (int[,])existingMap.Clone();
        }

        #endregion //Constructors

        //Returns the index of the texture in the tileTextures list
        //Returns -1 if the tile has not yet been used
        public int IsUsingTexture(Texture2D texture)
        {
            if (tileTextures.Contains(texture))
                return tileTextures.IndexOf(texture);

            return -1;
        }

        #region Save
        
        //Write the map layer to a file
        public void Save(string filename, string[] textureNames)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("[Textures]");
                foreach (string t in textureNames)
                    writer.WriteLine(t);

                writer.WriteLine();

                writer.WriteLine("[Properties]");
                writer.WriteLine("Alpha = " + Alpha.ToString());

                writer.WriteLine();

                writer.WriteLine("[Layout]");

                for (int y = 0; y < Height; y++)
                {
                    string line = string.Empty;
                    for (int x = 0; x < Width; x++)
                    {
                        line += map[y, x].ToString() + " ";
                    }
                    writer.WriteLine(line);
                }
            }
        }

        #endregion //Save

        #region Load

        //Loads a map layer from a file and returns a TileLayer
        public static TileLayer FromFile(ContentManager content, string filename)
        {
            TileLayer tileLayer;
            
            List<string> textureNames = new List<string>();

            tileLayer = ProcessFile(filename, textureNames);

            tileLayer.LoadTileTextures(content, textureNames.ToArray());

            return tileLayer;
        }

        //Loads a map layer from a file and returns both a TileLayer and the texture names used in the map layer 
        public static TileLayer FromFile(string filename, out string[] textureNameArray)
        {
            TileLayer tileLayer;
            
            List<string> textureNames = new List<string>();

            tileLayer = ProcessFile(filename, textureNames);

            textureNameArray = textureNames.ToArray();

            return tileLayer;
        }

        private static TileLayer ProcessFile(string filename, List<string> textureNames)
        {
            TileLayer tileLayer;
            List<List<int>> tempLayout = new List<List<int>>();
            Dictionary<string, string> propertiesDict = new Dictionary<string, string>();
            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingTextures = false;
                bool readingLayout = false;
                bool readingProperties = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Textures]"))
                    {
                        readingTextures = true;
                        readingLayout = false;
                        readingProperties = false;
                    }
                    else if (line.Contains("[Layout]"))
                    {
                        readingTextures = false;
                        readingLayout = true;
                        readingProperties = false;
                    }
                    else if(line.Contains("[Properties]"))
                    {
                        readingProperties = true;
                        readingTextures = false;
                        readingLayout = false;                        
                    }
                    else if (readingTextures)
                    {
                        textureNames.Add(line);
                    }
                    else if (readingLayout)
                    {
                        List<int> row = new List<int>();

                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))
                                row.Add(int.Parse(c));
                        }

                        tempLayout.Add(row);
                    }
                    else if (readingProperties)
                    {
                        string[] pair = line.Split('=');
                        string key = pair[0].Trim();
                        string value = pair[1].Trim();

                        propertiesDict.Add(key, value);
                    }
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            tileLayer = new TileLayer(width, height);

            foreach (KeyValuePair<string, string> property in propertiesDict)
            {
                switch (property.Key)
                {
                    case "Alpha":
                        tileLayer.Alpha = float.Parse(property.Value);
                        break;
                }
            }

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tileLayer.SetCellIndex(x, y, tempLayout[y][x]);
            return tileLayer;
        }

        public void LoadTileTextures(ContentManager content, params string[] textureNames)
        {
            Texture2D texture;
            foreach (string textureName in textureNames)
            {
                texture = content.Load<Texture2D>(textureName);
                tileTextures.Add(texture);
            }
        }

        #endregion //Load

        public void AddTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        public void RemoveTexture(Texture2D texture)
        {
            RemoveIndex(tileTextures.IndexOf(texture));
            tileTextures.Remove(texture);
        }

        //Used to add of remove tiles during runtime.
        public void SetCellIndex(int x, int y, int cellIndex) 
        {
            map[y, x] = cellIndex;
        }

        //Returns value at that point in the map
        public int GetCellIndex(int x, int y)
        {
            return map[y, x];
        }

        public void RemoveIndex(int existingIndex)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map[y, x] == existingIndex)
                        map[y, x] = -1;
                    else if (map[y, x] > existingIndex)
                        map[y, x]--;
                }
            }
        }

        public void ReplaceIndex(int existingIndex, int newIndex)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (map[y, x] == existingIndex)
                        map[y, x] = newIndex;
        }

        
        //Draws the TileLayer
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];

                    if (textureIndex == -1)
                        continue;

                    Texture2D texture = tileTextures[textureIndex];

                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            x * tileWidth - (int)camera.Position.X,
                            y * tileHeight - (int)camera.Position.Y,
                            tileWidth,
                            tileHeight),
                        new Color(new Vector4(1f, 1f, 1f, Alpha)));
                }
            }

            spriteBatch.End();
        }
    }
}
