using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace ArchanistTower
{
    public class Level
    {
        List<Map> mapList;
        private int currentMap;
        private int startMap;
        int[,] tileType;

        public int CurrentMap
        {
            get { return currentMap; }
            set
            {
                currentMap = (int)MathHelper.Clamp(value, 0, mapList.Count - 1);
            }
        }

        public int StartMap
        {
            get { return startMap; }
            set
            {
                startMap = value;
            }
        }

        public int MapWidthInPixels
        {
            get { return mapList[currentMap].Width * mapList[currentMap].TileWidth; }
        }

        public int MapHeightInPixels
        {
            get { return mapList[currentMap].Height * mapList[currentMap].TileHeight; }
        }

        public int TileWidth
        {
            get { return mapList[currentMap].TileWidth; }
        }

        public int TileHeight
        {
            get { return mapList[currentMap].TileHeight; }
        }
    
        public Level()
        {
            mapList = new List<Map>();
        }

        private Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)mapList[CurrentMap].TileWidth),
                (int)(position.Y / (float)mapList[CurrentMap].TileHeight));
        }

        public Rectangle ConvertRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * mapList[CurrentMap].TileWidth,
                cell.Y * mapList[CurrentMap].TileHeight,
                mapList[CurrentMap].TileWidth,
                mapList[CurrentMap].TileHeight);
        }

        public void AddMap(Map map)
        {
            mapList.Add(map);
        }       

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {            
            mapList[CurrentMap].Draw(spriteBatch);
        }

        public Vector2 CollisionCheck(Vector2 position)
        {
            Point spriteCell = ConvertPositionToCell(position);

            Point? upLeft = null, up = null, upRight = null,
                left = null, right = null, downLeft = null,
                down = null, downRight = null;

            if (spriteCell.Y > 0)
                up = new Point(spriteCell.X, spriteCell.Y - 1);

            if (spriteCell.Y < mapList[currentMap].Height - 1)
                down = new Point(spriteCell.X, spriteCell.Y + 1);

            if (spriteCell.X > 0)
                left = new Point(spriteCell.X - 1, spriteCell.Y);

            if (spriteCell.X < mapList[currentMap].Width - 1)
                right = new Point(spriteCell.X + 1, spriteCell.Y);

            if (spriteCell.X > 0 && spriteCell.Y > 0)
                upLeft = new Point(spriteCell.X - 1, spriteCell.Y - 1);

            if (spriteCell.X < mapList[currentMap].Width - 1 && spriteCell.Y > 0)
                upRight = new Point(spriteCell.X + 1, spriteCell.Y - 1);

            if (spriteCell.X > 0 && spriteCell.Y < mapList[currentMap].Height - 1)
                downLeft = new Point(spriteCell.X - 1, spriteCell.Y + 1);

            if (spriteCell.X < mapList[currentMap].Width - 1 &&
                spriteCell.Y < mapList[currentMap].Height - 1)
                downRight = new Point(spriteCell.X + 1, spriteCell.Y + 1);

            TileLayer l = mapList[currentMap].GetLayer("Collision") as TileLayer;
            bool solid = bool.Parse((string)l.Tiles[0,0].Properties["Solid"]);

            bool solid = bool.Parse((string)mapList[currentMap].GetLayer("Collision").

            if (!solid)
                return position;
            else
            {

            }

            
        }


    }
}
