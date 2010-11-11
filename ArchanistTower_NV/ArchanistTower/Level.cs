using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using ArchanistTower.GameObjects;
using ArchanistTower.Screens;

namespace ArchanistTower
{
    public class Level
    {
        protected Map currentMap;
        protected string mapFile;

        public Map CurrentMap
        {
            get { return currentMap; }
            set
            {
                currentMap = value;
            }
        }
        
        public int TileWidth
        {
            get { return CurrentMap.TileWidth; }
        }

        public int TileHeight
        {
            get { return CurrentMap.TileHeight; }
        }

        public string MapFile
        {
            get { return mapFile; }
            set { mapFile = value; }
        }

        public int MapWidthInPixels
        {
            get { return CurrentMap.Width * TileWidth; }
        }

        public int MapHeightInPixels
        {
            get { return CurrentMap.Height * TileHeight; }
        }
    
        public Level(string file)
        {
            MapFile = file;

        }

        private void LoadMap(sting file)
        {

        }

        public void LoadMap(string file, int startPoint, Player p)
        {
            MapFile = file;
            CurrentMap = Globals.content.Load<Map>(MapFile);

            GameScreen.gameWorld.gameObjects.Clear();

            GameScreen.gameWorld.gameObjects.Add(p);
        }

        private Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / TileWidth),
                (int)(position.Y / TileHeight));
        }

        public Rectangle CreateRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight,
                TileWidth,
                TileHeight);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            CurrentMap.Draw(spriteBatch);
            spriteBatch.End();
        }

        public AnimatedSprite CollisionCheck(AnimatedSprite inSprite)
        {
            Vector2 p = inSprite.Center;
            Point spriteCell = ConvertPositionToCell(p);

            Point? upLeft = null, up = null, upRight = null,
                left = null, right = null, downLeft = null,
                down = null, downRight = null;

            if (spriteCell.Y > 0)
                up = new Point(spriteCell.X, spriteCell.Y - 1);

            if (spriteCell.Y < CurrentMap.Height - 1)
                down = new Point(spriteCell.X, spriteCell.Y + 1);

            if (spriteCell.X > 0)
                left = new Point(spriteCell.X - 1, spriteCell.Y);

            if (spriteCell.X < CurrentMap.Width - 1)
                right = new Point(spriteCell.X + 1, spriteCell.Y);

            if (spriteCell.X > 0 && spriteCell.Y > 0)
                upLeft = new Point(spriteCell.X - 1, spriteCell.Y - 1);

            if (spriteCell.X < CurrentMap.Width - 1 && spriteCell.Y > 0)
                upRight = new Point(spriteCell.X + 1, spriteCell.Y - 1);

            if (spriteCell.X > 0 && spriteCell.Y < CurrentMap.Height - 1)
                downLeft = new Point(spriteCell.X - 1, spriteCell.Y + 1);

            if (spriteCell.X < CurrentMap.Width - 1 &&
                spriteCell.Y < CurrentMap.Height - 1)
                downRight = new Point(spriteCell.X + 1, spriteCell.Y + 1);

            TileLayer l = CurrentMap.GetLayer("Layer 1") as TileLayer;

            if (up != null && (string)l.Tiles[up.Value.X, up.Value.Y].Properties["TileType"] == "1")
            {
                Rectangle cellRect = CreateRectForCell(up.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.Y = spriteCell.Y * TileHeight;
                }
            }
            if (down != null && (string)l.Tiles[down.Value.X, down.Value.Y].Properties["TileType"] == "1")
            {
                Rectangle cellRect = CreateRectForCell(down.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.Y = down.Value.Y * TileHeight - inSprite.Bounds.Height;
                }
            }
            if (left != null && (string)l.Tiles[left.Value.X, left.Value.Y].Properties["TileType"] == "1")
            {
                Rectangle cellRect = CreateRectForCell(left.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = spriteCell.X * TileWidth;
                }
            }
            if (right != null && (string)l.Tiles[right.Value.X, right.Value.Y].Properties["TileType"] == "1")
            {
                Rectangle cellRect = CreateRectForCell(right.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = right.Value.X * TileWidth - inSprite.Bounds.Width;
                }
            }
            /*if (upLeft != null && (string)l.Tiles[upLeft.Value.X, upLeft.Value.Y].Properties["TileType"] == "1")
            {
                Rectangle cellRect = CreateRectForCell(right.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = spriteCell.X * TileWidth;
                    inSprite.Position.Y = spriteCell.Y * TileHeight;
                }
            }*/
            if (upRight != null && (string)l.Tiles[upRight.Value.X, upRight.Value.Y].Properties["TileType"] == "1")
            {
                Rectangle cellRect = CreateRectForCell(upRight.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = right.Value.X * TileWidth - inSprite.Bounds.Width;
                    inSprite.Position.Y = spriteCell.Y * TileHeight;
                }
            }
            if (downLeft != null && (string)l.Tiles[downLeft.Value.X, downLeft.Value.Y].Properties["TileType"] == "1")
            {
                Rectangle cellRect = CreateRectForCell(downLeft.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = spriteCell.X * TileWidth;
                    inSprite.Position.Y = down.Value.Y * TileHeight - inSprite.Bounds.Height;
                }
            }
            if (downRight != null && (string)l.Tiles[downRight.Value.X, downRight.Value.Y].Properties["TileType"] == "1")
            {
                Rectangle cellRect = CreateRectForCell(downRight.Value);
                Rectangle spriteRect = inSprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    inSprite.Position.X = right.Value.X * TileWidth - inSprite.Bounds.Width;
                    inSprite.Position.Y = down.Value.Y * TileHeight - inSprite.Bounds.Height;
                }
            }

            return inSprite;
        }

        public AnimatedSprite MapStartPosition(AnimatedSprite inSprite)
        {
            MapObject startPoint;
            MapObjectLayer obj = CurrentMap.GetLayer("Object Layer 1") as MapObjectLayer;
            startPoint = obj.GetObject("Start");
            inSprite.Position.X = startPoint.Bounds.X;
            inSprite.Position.Y = startPoint.Bounds.Y;
            return inSprite;
        }

    }
}
