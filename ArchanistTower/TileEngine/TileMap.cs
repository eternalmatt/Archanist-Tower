using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public class TileMap
    {
        public List<TileLayer> Layers = new List<TileLayer>();

        public int GetWidthInPixels()
        {
            return GetWidth() * Engine.TileWidth;
        }

        public int GetHeightInPixels()
        {
            return GetHeight() * Engine.TileHeight;
        }

        public int GetWidth()
        {
            int width = -10000;

            foreach (TileLayer layer in Layers)
                width = (int)Math.Max(width, layer.Width);

            return width;
        }

        public int GetHeight()
        {
            int height = -10000;

            foreach (TileLayer layer in Layers)
                height = (int)Math.Max(height, layer.Height);

            return height;
        }

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (TileLayer layer in Layers)
                layer.Draw(spriteBatch, camera);
        }
    }
}
