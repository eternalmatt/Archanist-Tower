using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower.GameObjects
{
    public class WindSpell : Spell
    {
        public WindSpell(FacingDirection fd, Vector2 pPosition)
        {
            SpellOrigin = pPosition;
            Direction = fd;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            FrameAnimation cast = new FrameAnimation(4, 16, 16, 64, 0);
            cast.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Cast", cast);
            SpriteAnimation.CurrentAnimationName = "Cast";
            SpriteAnimation.Speed = 5;
        }

        public override void Collision(GameObject o)
        {
            if (o is Enemy)
            {
                if (o is FireEnemy || o is FireBoss)
                {
                    o.Health -= 25;
                }
                Dead = true;
            }
        }
    }
}
