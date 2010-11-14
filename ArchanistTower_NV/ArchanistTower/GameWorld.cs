using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArchanistTower.GameObjects;
using TiledLib;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower
{
    public class GameWorld
    {
        Map map;
        public List<GameObject> GameObjects;
        ShaderCode shader = new ShaderCode();

        public int TileWidth
        {
            get { return map.TileWidth; }
        }

        public int TileHeight
        {
            get { return map.TileHeight; }
        }

        public int MapWidthInPixels
        {
            get { return map.Width * TileWidth; }
        }

        public int MapHeightInPixels
        {
            get { return map.Height * TileHeight; }
        }

        public GameWorld()
        {
            GameObjects = new List<GameObject>();
            shader.Initialize();
            shader.LoadContent();
        }

        public void Initialize()
        {
            shader.Initialize();
            shader.LoadContent();
            LoadMap("Levels//TestMap//TestMap");
        }

        public void AddObject(GameObject obj)
        {
            GameObjects.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update(gameTime);
                foreach (Rectangle clip in Level.ClipMap.Values)
                    if (GameObjects[i].SpriteAnimation.Bounds.Intersects(clip))
                    {
                        GameObjects[i].WorldCollision();
                        break;
                    }
                if (GameObjects[i].GetType() == typeof(Player))
                {
                    for (int j = 0; j < GameObjects.Count; j++)
                        if (GameObjects[j].GetType() == typeof(FireEnemy))
                            GameObjects[j].PlayerPosition = GameObjects[i].SpriteAnimation.Position;
                    foreach (Portal portal in Level.Portals)
                    {
                        if (GameObjects[i].SpriteAnimation.Bounds.Intersects(portal.Bounds))
                        {
                            LoadMap(portal.DestinationMap);
                            GameObjects[i].SpriteAnimation.Position = new Vector2(
                                (portal.DestinationTileLocation.X * map.TileWidth) + (map.TileWidth / 2),
                                (portal.DestinationTileLocation.X * map.TileHeight) + (map.TileHeight / 2));
                        }
                    }
                }
                //Makes as many inexpensive checks before checking intersection
                if (GameObjects[i].Collidable)
                    for (int j = 0; j < GameObjects.Count; j++)
                        if (i != j && GameObjects[j].Collidable)
                            if (Math.Abs(GameObjects[i].SpriteAnimation.Position.X - GameObjects[j].SpriteAnimation.Position.X) <= GameObjects[i].CollisionRadius &&
                               Math.Abs(GameObjects[i].SpriteAnimation.Position.Y - GameObjects[j].SpriteAnimation.Position.Y) <= GameObjects[i].CollisionRadius)
                                if (GameObjects[i].SpriteAnimation.Bounds.Intersects(GameObjects[j].SpriteAnimation.Bounds))
                                    if (PerPixelCollision(GameObjects[i].SpriteAnimation.Bounds, GameObjects[i].SpriteAnimation.SpriteTexture, GameObjects[j].SpriteAnimation.Bounds, GameObjects[j].SpriteAnimation.SpriteTexture))
                                        GameObjects[i].Collision(GameObjects[j]);
                
                if (GameObjects[i].Dead)
                    GameObjects.RemoveAt(i);
            }

        }

        #region PerPixelCollision
        private bool PerPixelCollision(Rectangle rectangleA, Texture2D textureA, Rectangle rectangleB, Texture2D textureB)
        {
            int areaA = textureA.Width * textureA.Height;
            int areaB = textureB.Width * textureB.Height;
            Color[] dataA = new Color[areaA];
            Color[] dataB = new Color[areaB];
            textureA.GetData(0, null, dataA, 0, areaA);
            textureB.GetData(0, null, dataB, 0, areaB);

            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
                for (int x = left; x < right; x++)
                    if (dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width].A != 0 &&
                        dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width].A != 0)
                        return true;
            /* I modified this section to sacrifice readability for better CPU by avoiding 
             * the repeated copying of large arrays of objects.
            // Get the color of both pixels at this point
            Color colorA = dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
            Color colorB = dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];
                    
            // If both pixels are not completely transparent,
            if (colorA.A != 0 && colorB.A != 0)
            {
                return true;// then an intersection has been found
            }
            */
            return false;// No intersection found
        }
        #endregion


        public void Draw()
        {
            //shader.DrawSetup();
            map.Draw(Globals.spriteBatch);
            for (int i = 0; i < GameObjects.Count; i++)
                GameObjects[i].Draw();            
            //shader.Draw();
        }

        #region Load

        private void LoadMap(string fileName)
        {
            Level.Portals = new List<Portal>();
            Level.ClipMap = new Dictionary<Vector2, Rectangle>();
            Level.Enemies = new List<Enemy>();
            if (Level.Player != null)
            {
                Level.Player = (Player)GameObjects[0];
                GameObjects.Clear();
                GameObjects.Add(Level.Player);
            }
            map = Globals.content.Load<Map>(fileName);

            MapObjectLayer objects = map.GetLayer("Objects") as MapObjectLayer;

            foreach (MapObject obj in objects.Objects)
            {
                switch (obj.Name)
                {
                    case "PlayerStart":
                        if (Level.Player == null)
                        {
                            Level.Player = new Player(new Vector2(obj.Bounds.X, obj.Bounds.Y));
                            GameObjects.Add((Player)Level.Player);
                            //gameObjects.Add(new FireEnemy(new Vector2(obj.Bounds.X, obj.Bounds.Y)));
                        }
                        break;

                    case "Portal":
                        Portal portal = new Portal();
                        portal.Bounds = obj.Bounds;
                        Property tempP = obj.Properties["DestinationMap"];
                        portal.DestinationMap = tempP.RawValue;
                        //portal.DestinationMap = (string)obj.Properties["DestinationMap"];
                        tempP = obj.Properties["DestinationX"];
                        string tileLocX = tempP.RawValue; //obj.Properties["DestinationX"].RawValue;
                        string tileLocY = obj.Properties["DestinationY"].RawValue;
                        portal.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLocX), Convert.ToInt32(tileLocY));
                        Level.Portals.Add(portal);
                        break;
                }
            }
            LoadClipMap();
            LoadEnemies();
        }

        private void LoadClipMap()
        {
            Level.ClipMap = new Dictionary<Vector2, Rectangle>();
            TileLayer clipLayer = map.GetLayer("Clip") as TileLayer;
            for (int y = 0; y < clipLayer.Width; y++)
                for (int x = 0; x < clipLayer.Height; x++)
                {
                    Tile tile = clipLayer.Tiles[x, y];
                    if (tile != null)
                        Level.ClipMap.Add(new Vector2(x, y), new Rectangle(x * tile.Source.Width, y * tile.Source.Height, tile.Source.Width, tile.Source.Height));
                }
            map.GetLayer("Clip").Visible = false; 
        }

        private void LoadEnemies()
        {
            Level.Enemies = new List<Enemy>();
            TileLayer enemyLayer = map.GetLayer("Enemy") as TileLayer;
            for (int y = 0; y < enemyLayer.Width; y++)
                for (int x = 0; x < enemyLayer.Height; x++)
                {
                    Tile tile = enemyLayer.Tiles[x, y];
                    if (tile != null)
                    {
                        string enemyType = tile.Properties["Type"].RawValue;
                        int enemyX = x * map.TileWidth;
                        int enemyY = y * map.TileHeight;
                        if (enemyType == "Fire")
                            GameObjects.Add(new FireEnemy(new Vector2(enemyX, enemyY)));                                          
                    }
                }
            
        }

        #endregion //Load

    }
}
