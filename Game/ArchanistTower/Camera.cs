using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArchanistTower
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;

        public Matrix TransformMatrix
        {
            get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)); }
        }

        public void LockToTarget(AnimatedSprite sprite, int screenWidth, int screenHeight)
        {
            Position.X =
                sprite.Position.X +
                (sprite.CurrentAnimation.CurrentRect.Width / 2) -
                (screenWidth / 2);
            Position.Y =
                sprite.Position.Y +
                (sprite.CurrentAnimation.CurrentRect.Height / 2) -
                (screenHeight / 2);
        }

        public void ClampToArea(int width, int height)
        {
            //Restrict where the camera can go. (i.e. not off the map)
            if (Position.X > width)
                Position.X = width;
            if (Position.Y > height)
                Position.Y = height;

            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;
        }
    }
}
