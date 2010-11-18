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

        public override void Initialize()
        {
            SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Spells/spellsprites"));

            FrameAnimation cast = new FrameAnimation(4, 16, 16, 0, 0);
            cast.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Cast", cast);
            SpriteAnimation.CurrentAnimationName = "Cast";
            SpriteAnimation.Speed = 3;

            motion = Vector2.Zero;

            if(Direction == FacingDirection.Down)
                motion.Y = 1;
            else if(Direction == FacingDirection.Up)
                motion.Y = -1;
            else if(Direction == FacingDirection.Left)
                motion.X = -1;
            else if(Direction == FacingDirection.Right)
                motion.X = 1;
            motion.Normalize();
            SpriteAnimation.Position = SpellOrigin;
            Collidable = true;
            CollisionRadius = 32;

        }

        public override void Update(GameTime gameTime)
        {
            if (!Dead)
                SpriteAnimation.Position += SpriteAnimation.Speed * motion;
            SpriteAnimation.Update(gameTime);
        }

        public override void Collision(GameObject o)
        {
            if (o is Enemy)
            {
                if (o is FireEnemy || o is FireBoss)
                {
                    o.Health -= 35;
                }
                Dead = true;
            }
        }
        
    }
}
