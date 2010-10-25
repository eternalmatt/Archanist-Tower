using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower
{
    public static class HUD
    {
        struct HUDItem
        {
            public SpriteFont font;
            public String text;
            public Vector2 position;
            public Color color;
        }
        static List<HUDItem> HUDlist = new List<HUDItem>();

        public static void NewHUDItem(SpriteFont font, String text, Vector2 position, Color color)
        {
            HUDItem newItem;
            newItem.font = font;
            newItem.text = text;
            newItem.position = position;
            newItem.color = color;
            HUDlist.Add(newItem);
        }


        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in HUDlist)
                spriteBatch.DrawString(item.font, item.text, item.position, item.color);
        }
    }
}
