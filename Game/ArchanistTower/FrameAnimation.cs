using System;
using Microsoft.Xna.Framework;

namespace ArchanistTower
{
    public class FrameAnimation : ICloneable
    {
        Rectangle[] frames;
        int currentFrame = 0;

        float frameLength = .5f;
        float timer = 0;

        public int FramesPerSecond
        {
            get { return (int)(1f / frameLength); }
            set { frameLength = (float)Math.Max(1f / (float)value, .01f); } 
        }

        public Rectangle CurrentRect
        {
            get { return frames[currentFrame]; }
        }

        public int CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = (int)MathHelper.Clamp(value, 0, frames.Length - 1); }
        }

        public FrameAnimation(int numberOfFrames, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            frames = new Rectangle[numberOfFrames];
            for (int i = 0; i < numberOfFrames; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = frameWidth;
                rect.Height = frameHeight;
                rect.X = xOffset + (i * frameWidth);
                rect.Y = yOffset;

                frames[i] = rect;
            }
        }

        private FrameAnimation()
        {
        }

        public void Update(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer >= frameLength)
            {
                timer = 0f;
//                currentFrame++;
  //              if (CurrnetFrame >= frames.Length)
    //                currentFrame = 0;
                currentFrame = (currentFrame + 1) % frames.Length;
            }
        }

        public object Clone()
        {
            FrameAnimation anim = new FrameAnimation();
            anim.frameLength = frameLength;
            anim.frames = frames;

            return anim;
        }

    }
}
