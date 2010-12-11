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
            if (o is Enemy && originatingType == OriginatingType.Player)
            {
                o.Health -= 20;
                //switch (Direction) // stuns the enemy backwards for 20 units
                //{
                //    case FacingDirection.Left:
                //        o.SpriteAnimation.Position.X -= 20;
                //        o.WorldCollision();
                //        break;
                //    case FacingDirection.Right:
                //        o.SpriteAnimation.Position.X += 20;
                //        o.WorldCollision();
                //        break;
                //    case FacingDirection.Up:
                //        o.SpriteAnimation.Position.Y -= 20;
                //        o.WorldCollision();
                //        break;
                //    case FacingDirection.Down:
                //        o.SpriteAnimation.Position.Y += 20;
                //        o.WorldCollision();
                //        break;
                //}
                checkClipCollision(this, o);
                Dead = true;
            }
            else if (o is Player && originatingType == OriginatingType.Enemy)
            {
                o.Health -= 5;
                //switch (Direction) // stuns the player backwards for 20 units
                //{
                //    case FacingDirection.Left:
                //        o.SpriteAnimation.Position.X -= 20;
                //        break;
                //    case FacingDirection.Right:
                //        o.SpriteAnimation.Position.X += 20;
                //        break;
                //    case FacingDirection.Up:
                //        o.SpriteAnimation.Position.Y -= 20;
                //        break;
                //    case FacingDirection.Down:
                //        o.SpriteAnimation.Position.Y += 20;
                //        break;
                //}
                checkClipCollision(this, o);
                Dead = true;
            }
        }

        private void checkClipCollision(GameObject oHits, GameObject oGotHit)
        {
            Rectangle futureRectangle = oGotHit.SpriteAnimation.Bounds;

            switch (oHits.Direction)
            {
                case FacingDirection.Left:
                    futureRectangle.X -= 20;
                    foreach (Rectangle clip in GameWorld.ClipMap.Values)
                        if (clip.Intersects(futureRectangle))
                            {
                                oGotHit.SpriteAnimation.Position.X = clip.Right + 5;
                                return;
                            }
                    oGotHit.SpriteAnimation.Position.X -= 20;
                    break;
                case FacingDirection.Right:
                    futureRectangle.X += 20;
                    foreach (Rectangle clip in GameWorld.ClipMap.Values)
                        if (clip.Intersects(futureRectangle))
                            {
                                oGotHit.SpriteAnimation.Position.X = clip.Left - 5 - oGotHit.SpriteAnimation.Bounds.Width;
                                return;
                            }
                    oGotHit.SpriteAnimation.Position.X += 20;
                    break;
                case FacingDirection.Up:
                    futureRectangle.Y -= 20;
                    foreach (Rectangle clip in GameWorld.ClipMap.Values)
                        if (clip.Intersects(futureRectangle))
                            {
                                oGotHit.SpriteAnimation.Position.Y = clip.Bottom + 5;
                                return;
                            }
                    oGotHit.SpriteAnimation.Position.Y -= 20;
                    break;
                case FacingDirection.Down:
                    futureRectangle.Y += 20;
                    foreach (Rectangle clip in GameWorld.ClipMap.Values)
                        if (clip.Intersects(futureRectangle))
                            {
                                oGotHit.SpriteAnimation.Position.Y = clip.Top - 5 - oGotHit.SpriteAnimation.Bounds.Height;
                                return;
                            }
                    oGotHit.SpriteAnimation.Position.Y += 20;
                    break;
            }
        }
    }
}
