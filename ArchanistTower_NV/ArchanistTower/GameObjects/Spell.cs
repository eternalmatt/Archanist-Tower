using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledLib;


namespace ArchanistTower.GameObjects
{
    public class Spell : GameObject
    {
        public FacingDirection Direction { get; set; }
        public Vector2 SpellOrigin { get; set; }
        public Vector2 Motion { get; set; }
        

        public override void Draw()
        {
            SpriteAnimation.Draw(Globals.spriteBatch);
        }

        public void Cast(string s, Vector2 p)
        {
            Vector2 offset;
            switch (s)
            {
                case "Up":
                    Motion = new Vector2(0, -1);
                    offset = new Vector2(8, -16);
                    break;
                case "Down":
                    Motion = new Vector2(0, 1);
                    offset = new Vector2(8, 24);
                    break;
                case "Left":
                    Motion = new Vector2(-1, 0);
                    offset = new Vector2(0, 16);
                    break;
                case "Right":
                    Motion = new Vector2(1, 0);
                    offset = new Vector2(24, 16);
                    break;
                default:
                    Motion = Vector2.Zero;
                    offset = Vector2.Zero;
                    break;
            }
            SpriteAnimation.Position = p + offset;
        }
    }
}



