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

        private Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / (float)mapList[CurrentMap].TileWidth),
                (int)(position.Y / (float)mapList[CurrentMap].TileHeight));
        }

        public Rectangle CreateRectForCell(Point cell)
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

        public AnimatedSprite CollisionCheck(AnimatedSprite inSprite)
        {
            Vector2 p = inSprite.Position;
            Point spriteCell = ConvertPositionToCell(p);

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
            int z = int.Parse((string)l.Tiles[down.Value.X, down.Value.Y].Properties["Solid"]);

            if (up != null && int.Parse((string)l.Tiles[up.Value.X, up.Value.Y].Properties["Solid"]) == 1)
            {
                Rectangle cellRect = CreateRectForCell(up.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.Y = spriteCell.Y * TileHeight;
                }
            }
            if (down != null)
            {
                int i = int.Parse((string)l.Tiles[down.Value.X, down.Value.Y].Properties["Solid"]);
                if(i == 1)
                {
                    Rectangle cellRect = CreateRectForCell(down.Value);
                    Rectangle spriteRect = inSprite.Bounds;
                    if (cellRect.Intersects(spriteRect))
                    {
                        inSprite.Position.Y =
                            down.Value.Y * TileHeight - inSprite.Bounds.Height;
                    }
                }
            }
            if (left != null && bool.Parse((string)l.Tiles[left.Value.X, left.Value.Y].Properties["Solid"]))
            {
                Rectangle cellRect = CreateRectForCell(left.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = spriteCell.X * TileWidth;
                }
            }
            if (right != null && bool.Parse((string)l.Tiles[right.Value.X, right.Value.Y].Properties["Solid"]))
            {
                Rectangle cellRect = CreateRectForCell(right.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X =
                        right.Value.X * TileWidth - inSprite.Bounds.Width;
                }
            }
          /*if (upLeft != null && tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 1)
            {
                Rectangle cellRect = CreateRectForCell(right.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if(cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = spriteCell.X * TileWidth;
                    inSprite.Position.Y = spriteCell.Y * TileHeight;
                }
             }*/
            if (upRight != null && bool.Parse((string)l.Tiles[upRight.Value.X, upRight.Value.Y].Properties["Solid"]))
            {
                Rectangle cellRect = CreateRectForCell(upRight.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X =
                        right.Value.X * TileWidth - inSprite.Bounds.Width;
                    inSprite.Position.Y = spriteCell.Y * TileHeight;
                }
            }
            if (downLeft != null && bool.Parse((string)l.Tiles[downLeft.Value.X, downLeft.Value.Y].Properties["Solid"]))
            {
                Rectangle cellRect = CreateRectForCell(downLeft.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = spriteCell.X * TileWidth;
                    inSprite.Position.Y =
                        down.Value.Y * TileHeight - inSprite.Bounds.Height;
                }
            }
            if (downRight != null && bool.Parse((string)l.Tiles[downRight.Value.X, downRight.Value.Y].Properties["Solid"]))
            {
                Rectangle cellRect = CreateRectForCell(downRight.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X =
                        right.Value.X * TileWidth - inSprite.Bounds.Width;
                    inSprite.Position.Y =
                        down.Value.Y * TileHeight - inSprite.Bounds.Height;
                }
            }

            return inSprite;
        }

    }
}
