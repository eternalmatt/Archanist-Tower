using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower
{
    public class ResolutionManager
    {
        Vector2 Resolution;

        public ResolutionManager(Vector2 res)
        {
            Resolution = res;
        }

        public Vector2 GetResolution() { return Resolution; }
        public int GetWidth() { return (int)Resolution.X; }
        public int GetHeight() { return (int)Resolution.Y; }
        public Vector2 ObtainResolution() { return Resolution; }

        public static Vector2 Res800x600
        {
            get { return new Vector2(800, 600); }
        }

        public static Vector2 Res1280x720
        {
            get { return new Vector2(1280, 720); }
        }
    }
}
