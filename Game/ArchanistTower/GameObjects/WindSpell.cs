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

        /// <summary>
        /// Used for fireboss to throw a spell in all directions and not just cardinal
        /// </summary>
        /// <param name="towards">Direction spell will travel towards</param>
        /// <param name="from">Position spell originated (usually SpriteAnimation.Position)</param>
        public WindSpell(Vector2 towards, Vector2 from)
        {
            SpellOrigin = from;
            motion = towards - from;
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
                o.Health -= 20;
                switch (Direction) // stuns the enemy backwards for 20 units
                {
                    case FacingDirection.Left:
                        o.SpriteAnimation.Position.X -= 20;
                        break;
                    case FacingDirection.Right:
                        o.SpriteAnimation.Position.X += 20;
                        break;
                    case FacingDirection.Up:
                        o.SpriteAnimation.Position.Y -= 20;
                        break;
                    case FacingDirection.Down:
                        o.SpriteAnimation.Position.Y += 20;
                        break;
                }
                Dead = true;
            }
            else if (o is Player && originatingType == OriginatingType.Boss)
            {
                o.Health -= 5;
                switch (Direction) // stuns the player backwards for 20 units
                {
                    case FacingDirection.Left:
                        o.SpriteAnimation.Position.X -= 20;
                        break;
                    case FacingDirection.Right:
                        o.SpriteAnimation.Position.X += 20;
                        break;
                    case FacingDirection.Up:
                        o.SpriteAnimation.Position.Y -= 20;
                        break;
                    case FacingDirection.Down:
                        o.SpriteAnimation.Position.Y += 20;
                        break;
                }
                Dead = true;
            }
        }
    }
}
