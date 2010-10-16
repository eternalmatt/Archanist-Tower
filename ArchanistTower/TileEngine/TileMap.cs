using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public class TileMap
    {
        public List<TileLayer> Layers = new List<TileLayer>();

        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (TileLayer layer in Layers)
                layer.Draw(spriteBatch, camera);
        }
    }
}
