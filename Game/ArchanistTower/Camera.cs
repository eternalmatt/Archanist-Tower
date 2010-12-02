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
        public const float ZOOM = 1.0f;

        public Matrix TransformMatrix
        {
            get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)) * Matrix.CreateScale(ZOOM); }
        }

        public void LockToTarget(AnimatedSprite sprite, int screenWidth, int screenHeight)
        {
            Position.X =
                sprite.Position.X +
                ((sprite.CurrentAnimation.CurrentRect.Width * ZOOM) / 2) -
                (screenWidth / 2) / ZOOM;
            Position.Y =
                sprite.Position.Y +
                ((sprite.CurrentAnimation.CurrentRect.Height * ZOOM) / 2) -
                (screenHeight / 2) / ZOOM;
        }

        public void ClampToArea(int width, int height)
        {
            //Restrict where the camera can go. (i.e. not off the map)
            if (Position.X > width + (((width * ZOOM) - width) / 2))
                Position.X = width + (((width * ZOOM) - width) / 2);
            if (Position.Y > height)// * ZOOM)
                Position.Y = height;// * ZOOM;

            if (Position.X < 0)
                Position.X = 0;
            if (Position.Y < 0)
                Position.Y = 0;
        }                
    }
}