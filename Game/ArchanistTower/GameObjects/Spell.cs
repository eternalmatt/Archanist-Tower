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
        public Vector2 motion = Vector2.Zero;

        public enum OriginatingType
        {
            Player,
            Boss
        }
        public OriginatingType originatingType;

        public override void Initialize()
        {
            SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Spells/spellsprites"));

            if (motion == Vector2.Zero)
                if (Direction == FacingDirection.Down)
                    motion.Y = 1;
                else if (Direction == FacingDirection.Up)
                    motion.Y = -1;
                else if (Direction == FacingDirection.Left)
                    motion.X = -1;
                else if (Direction == FacingDirection.Right)
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
            base.Update(gameTime);
        }

        public override void Draw()
        {
            if(!Dead)
                SpriteAnimation.Draw(Globals.spriteBatch);
        }

        public override void WorldCollision()
        {
            Dead = true;    
        }       
    }
}



