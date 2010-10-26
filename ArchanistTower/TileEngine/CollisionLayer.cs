using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public class CollisionLayer
    {
        #region Variables
    
        //The array of the layer map
        //Values of -1 will be treated as blanks spaces and values that are indexes of the tileTextures will draw the repective texture
        int[,] map;


        #endregion //Variables

        #region Properties

       
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
        public CollisionLayer(int width, int height)
        {
            map = new int[height, width];

            //Sets values of the map to -1 (all blank spaces)
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[y, x] = -1;
        }      

        #endregion //Constructors

        //Returns the index of the texture in the tileTextures list
        //Returns -1 if the tile has not yet been used
       

        #region Save
        
        //Write the map layer to a file
        public void Save(string filename, string[] textureNames)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
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

        public static CollisionLayer FromFile(string filename)
        {
            CollisionLayer CollisionLayer;
            List<List<int>> tempLayout = new List<List<int>>();
            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingLayout = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();

                    if (string.IsNullOrEmpty(line))
                        continue;

                    if (line.Contains("[Layout]"))
                    {
                        readingLayout = true;
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
                }
            }

            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            CollisionLayer = new CollisionLayer(width, height);
                        
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    CollisionLayer.SetCellIndex(x, y, tempLayout[y][x]);
            return CollisionLayer;
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

        public int GetCellIndex(Point point)
        {
            return map[point.Y, point.X];
        }

        public void SetCellIndes(Point point, int cellIndex)
        {
            map[point.Y, point.X] = cellIndex;
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
    }
}
