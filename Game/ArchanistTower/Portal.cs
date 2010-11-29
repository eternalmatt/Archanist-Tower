using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower
{
    public class Portal
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public string DestinationMap { get; set; }
        public Vector2 DestinationTileLocation { get; set; }
        public Rectangle Bounds { get; set; }
       
    }
}
