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

        public Point ConvertPositionToCell(Vector2 position)
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
            Globals.spriteBatch.Begin();
            mapList[CurrentMap].Draw(spriteBatch);
            Globals.spriteBatch.End();
        }
    }
}
