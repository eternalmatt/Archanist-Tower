using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public class TileLayer
    {
        static int tileWidth = 64;
        static int tileHeight = 64;

        public static int TileWidth
        {
            get { return tileWidth; }
            set
            {
                tileWidth = (int)MathHelper.Clamp(value, 20f, 100f);
            }
        }

        public static int TileHeight
        {
            get { return tileHeight; }
            set
            {
                tileHeight = (int)MathHelper.Clamp(value, 20f, 100f);
            }
        }
        List<Texture2D> tileTextures = new List<Texture2D>();
        int[,] map;

        public int WidthInPixels
        {
            get
            {
                return Width * tileWidth;
            }
        }

        public int HeightInPixels
        {
            get
            {
                return Height * tileHeight;
            }
        }

        public int Width
        {
            get { return map.GetLength(1); }
        }

        public int Height
        {
            get { return map.GetLength(0); }
        }

        public TileLayer(int width, int height)
        {
            map = new int[height, width];
        }

        public TileLayer(int[,] existingMap)
        {
            map = (int[,])existingMap.Clone();
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

        public void AddTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin();

            int tileMapWidth = map.GetLength(1);
            int tileMapHeight = map.GetLength(0);

            for (int x = 0; x < tileMapWidth; x++)
            {
                for (int y = 0; y < tileMapHeight; y++)
                {
                    int textureIndex = map[y, x];
                    Texture2D texture = tileTextures[textureIndex];

                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            x * tileWidth - (int)camera.Position.X,
                            y * tileHeight - (int)camera.Position.Y,
                            tileWidth,
                            tileHeight),
                        Color.White);
                }
            }

            spriteBatch.End();
        }
    }
}
