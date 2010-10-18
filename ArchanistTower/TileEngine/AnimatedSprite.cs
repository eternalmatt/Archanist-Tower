using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    public class AnimatedSprite
    {
        public Dictionary<string, FrameAnimation> Animations = 
            new Dictionary<string, FrameAnimation>();

        string currentAnimation = null;
        bool animating = true;
        Texture2D texture;

        public Vector2 Position = Vector2.Zero;        

        private float speed = 2f;
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
        }
    }
}
