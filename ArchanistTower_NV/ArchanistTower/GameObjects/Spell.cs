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
        public Vector2 SpellOrigin { get; set; }
        public Vector2 motion;

        public override void Draw()
        {
            SpriteAnimation.Draw(Globals.spriteBatch);
        }

        public override void WorldCollision()
        {
            Dead = true;    
        }

       
    }
}



