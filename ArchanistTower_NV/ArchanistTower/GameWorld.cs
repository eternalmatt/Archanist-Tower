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
            //shader.LoadContent();
        }

        public void Initialize()
        {
            //shader.Initialize();
            //LoadMap("Levels//TestMap//TestMap");
            //LoadMap("Levels//TestFireMap//Mountain1");
            LoadMap("Levels//TestFireMap//MountainEntrance");

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
                        if (portal.DestinationMap != "crys")
                        {
                            if (GameObjects[i].SpriteAnimation.Bounds.Intersects(portal.Bounds))
                            {
                                for (int j = 0; j < GameObjects.Count; j++)
                                    if (GameObjects[j].GetType() == typeof(FireEnemy))
                                        GameObjects[j].Dead = true;
                                LoadMap(portal.DestinationMap);
                                GameObjects[i].SpriteAnimation.Position = new Vector2(
                                    (portal.DestinationTileLocation.X * map.TileWidth) + (map.TileWidth / 2),
                                    (portal.DestinationTileLocation.Y * map.TileHeight) + (map.TileHeight / 2));
                            }
                        }
                        else
                        {
                            if (GameObjects[i].SpriteAnimation.Bounds.Intersects(portal.Bounds))
                            {
                                Globals.shading = portal.DestinationTileLocation.X;
                            }
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
;
            map.Draw(Globals.spriteBatch);
            for (int i = 0; i < GameObjects.Count; i++)
                GameObjects[i].Draw();            

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
                    case "Portal2":
                        Portal portal2 = new Portal();
                        portal2.Bounds = obj.Bounds;
                        Property tempP2 = obj.Properties["DestinationMap"];
                        portal2.DestinationMap = tempP2.RawValue;
                        //portal.DestinationMap = (string)obj.Properties["DestinationMap"];
                        tempP = obj.Properties["DestinationX"];
                        string tileLocX2 = tempP.RawValue; //obj.Properties["DestinationX"].RawValue;
                        string tileLocY2 = obj.Properties["DestinationY"].RawValue;
                        portal2.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLocX2), Convert.ToInt32(tileLocY2));
                        Level.Portals.Add(portal2);
                        break;
                    case "Portal3":
                        Portal portal3 = new Portal();
                        portal3.Bounds = obj.Bounds;
                        Property tempP3 = obj.Properties["DestinationMap"];
                        portal3.DestinationMap = tempP3.RawValue;
                        //portal.DestinationMap = (string)obj.Properties["DestinationMap"];
                        tempP3 = obj.Properties["DestinationX"];
                        string tileLocX3 = tempP3.RawValue; //obj.Properties["DestinationX"].RawValue;
                        string tileLocY3 = obj.Properties["DestinationY"].RawValue;
                        portal3.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLocX3), Convert.ToInt32(tileLocY3));
                        Level.Portals.Add(portal3);
                        break;
                    case "Portal4":
                        Portal portal4 = new Portal();
                        portal4.Bounds = obj.Bounds;
                        Property tempP4 = obj.Properties["DestinationMap"];
                        portal4.DestinationMap = tempP4.RawValue;
                        //portal.DestinationMap = (string)obj.Properties["DestinationMap"];
                        tempP4 = obj.Properties["DestinationX"];
                        string tileLocX4 = tempP4.RawValue; //obj.Properties["DestinationX"].RawValue;
                        string tileLocY4 = obj.Properties["DestinationY"].RawValue;
                        portal4.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLocX4), Convert.ToInt32(tileLocY4));
                        Level.Portals.Add(portal4);
                        break;
                    case "crys":
                        Portal crys = new Portal();
                        crys.Bounds = obj.Bounds;
                        Property tempP5 = obj.Properties["DestinationMap"];
                        crys.DestinationMap = tempP5.RawValue;
                        //portal.DestinationMap = (string)obj.Properties["DestinationMap"];
                        tempP5 = obj.Properties["DestinationX"];
                        string tileLocX5 = tempP5.RawValue; //obj.Properties["DestinationX"].RawValue;
                        string tileLocY5 = obj.Properties["DestinationY"].RawValue;
                        crys.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLocX5), Convert.ToInt32(tileLocY5));
                        Level.Portals.Add(crys);
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
            map.GetLayer("Enemy").Visible = false; 
        }

        #endregion //Load

    }
}
