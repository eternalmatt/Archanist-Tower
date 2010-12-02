using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower.GameObjects
{
    public class FireSpell : Spell
    {
        public FireSpell(FacingDirection fd, Vector2 pPosition)
        {
            SpellOrigin = pPosition;
            Direction = fd;
            Initialize();
        }

        /// <summary>
        /// Used for fireboss to throw a spell in all directions and not just cardinal
        /// </summary>
        /// <param name="towards">Direction spell will travel towards</param>
        /// <param name="from">Position spell originated (usually SpriteAnimation.Position)</param>
        public FireSpell(Vector2 towards, Vector2 from)
        {
            SpellOrigin = from;
            motion = from - towards;
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            FrameAnimation cast = new FrameAnimation(4, 16, 16, 0, 0);
            cast.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Cast", cast);
            SpriteAnimation.CurrentAnimationName = "Cast";
            SpriteAnimation.Speed = 3;
        }

        public override void Collision(GameObject o)
        {
            if (o is Enemy)
            {
                if (originatingType == OriginatingType.Player)
                    if (o is FireEnemy || o is FireBoss)
                    {
                        o.Health -= 35;
                    }
                Dead = true;
            }
            else if (o is Player && originatingType == OriginatingType.Boss)
            {
                o.Health -= 10;
                Dead = true;
            }
        }
        
    }
}
