using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower
{
    public class AnimatedSprite
    {
        public Dictionary<string, FrameAnimation> Animations = 
            new Dictionary<string, FrameAnimation>();

        private float speed = 2f;
        string currentAnimation = null;
        bool animating = true;
        Texture2D texture;
        public Color[] data { get; set; }
        //public Texture2D SpriteTexture { get { return texture; } }

        public Vector2 Position = Vector2.Zero;
        public Vector2 OriginOffset = Vector2.Zero;

        public Vector2 Origin
        {
            get { return Position + OriginOffset; }
        }

        public Vector2 Center
        {
            get
            {
                return Position + new Vector2(
                    CurrentAnimation.CurrentRect.Width / 2,
                    CurrentAnimation.CurrentRect.Height / 2);
            }
        }

        public float Bottom
        {
            get { return Position.Y; }
        }

        public float Top
        {
            get { return Position.Y + CurrentAnimation.CurrentRect.Height; }
        }

        public float Left
        {
            get { return Position.X; } 
        }

        public float Right
        {
            get { return Position.X + CurrentAnimation.CurrentRect.Width; }
        }

        public Rectangle Bounds
        {
            get
            {
                Rectangle rect = CurrentAnimation.CurrentRect;
                rect.X = (int)Position.X;
                rect.Y = (int)Position.Y;
                return rect;
            }
        }


        public float Speed
        {
            get { return speed; }
            set
            {
                speed = (float)Math.Max(value, .1f);
            }
        }

        public bool IsAnimating
        {
            get { return animating; }
            set { animating = value; }
        }

        public FrameAnimation CurrentAnimation
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAnimation))
                    return Animations[currentAnimation];
                else
                    return null;
            }
        }

        public string CurrentAnimationName
        {
            get { return currentAnimation; }
            set
            {
                if (Animations.ContainsKey(value))
                    currentAnimation = value;
            }
        }

        public AnimatedSprite(Texture2D texture)
        {
            this.texture = texture;
            int area = texture.Width * texture.Height;
            data = new Color[area];
            texture.GetData(0, null, data, 0, area);
        }

        public void ClampToArea(int width, int height)
        {
            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;
            if (Position.X > width - CurrentAnimation.CurrentRect.Width)
                Position.X = width - CurrentAnimation.CurrentRect.Width;            
            if (Position.Y > height - CurrentAnimation.CurrentRect.Height)
                Position.Y = height - CurrentAnimation.CurrentRect.Height;
        }

        public void Update(GameTime gameTime)
        {

            if (!IsAnimating)
                return;

            FrameAnimation animation = CurrentAnimation;

            if (animation == null)
            {
                if (Animations.Count > 0)
                {
                    string[] keys = new string[Animations.Count];
                    Animations.Keys.CopyTo(keys, 0);

                    currentAnimation = keys[0];

                    animation = CurrentAnimation;
                }
                else
                    return;
            }
            animation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            FrameAnimation animation = CurrentAnimation;

            if (animation != null)
                   spriteBatch.Draw(
                         texture,
                         Position,
                         animation.CurrentRect,
                         Color.White); 
               /* spriteBatch.Draw(
                    texture,
                    Position,
                    animation.CurrentRect,
                    Color.White,
                    0.0f, Vector2.Zero, 2.0f,
                    SpriteEffects.None,
                    0.0f);
                */
        }
    }
}
