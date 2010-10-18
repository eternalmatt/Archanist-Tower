using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;



        public Matrix TransformMatrix
        {
            get { return Matrix.CreateTranslation(new Vector3(-Position, 0f)); }
        }
    }
}
