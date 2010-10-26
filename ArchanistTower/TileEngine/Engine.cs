using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class Engine
    {
        //Drawing size of the tiles.
        public const int TileWidth = 32;
        public const int TileHeight = 32;

        public static Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)TileWidth),
                (int)(position.Y / (float)TileHeight));
        }

        public static Rectangle CreateRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight,
                TileWidth,
                TileHeight);
        }
    }
}

    

