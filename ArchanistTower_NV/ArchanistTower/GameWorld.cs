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

        //Map Things
        public static Map Map { get; set; }
        public static Dictionary<Vector2, Rectangle> ClipLayer { get; set; }
        public static List<Portal> Portals { get; set; }
        public  List<GameObject> GameObjects { get; set; }
        public static bool Debug { get; set; }
        //public List<GameObject> GameObjects;



        public int TileWidth
        {
            get { return Map.TileWidth; }
        }

        public int TileHeight
        {
            get { return Map.TileHeight; }
        }

        public int MapWidthInPixels
        {
            get { return Map.Width * TileWidth; }
        }

        public int MapHeightInPixels
        {
            get { return Map.Height * TileHeight; }
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
            Debug = false;

        }

        public void AddObject(GameObject obj)
        {
            GameObjects.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < GameObjects.Count; i++)
            {
                //update each obj
                GameObjects[i].Update(gameTime);  

                //Check for collision with clip layer
                foreach (Rectangle clip in ClipLayer.Values)
                    if (GameObjects[i].SpriteAnimation.Bounds.Intersects(clip))
                    {
                        GameObjects[i].WorldCollision();
                        break;
                    }
                if (GameObjects[i].GetType() == typeof(Player))
                {
                    for (int j = 0; j < GameObjects.Count; j++)
                        if (GameObjects[j].GetType() == typeof(FireEnemy) || GameObjects[j].GetType() == typeof(FireBoss))
                            GameObjects[j].PlayerPosition = GameObjects[i].SpriteAnimation.Position;

                    foreach (Portal portal in Portals)
                    {
                        if (portal.DestinationMap != "crys")
                        {
                            if (GameObjects[i].SpriteAnimation.Bounds.Intersects(portal.Bounds))
                            {
                                //for (int j = 0; j < GameObjects.Count; j++)
                                //    if (GameObjects[j].GetType() == typeof(FireEnemy) || GameObjects[j].GetType() == typeof(FireBoss))
                                //        GameObjects[j].Dead = true;
                                LoadMap(portal.DestinationMap);
                                GameObjects[i].SpriteAnimation.Position = new Vector2(
                                    (portal.DestinationTileLocation.X * Map.TileWidth) + (Map.TileWidth / 2),
                                    (portal.DestinationTileLocation.Y * Map.TileHeight) + (Map.TileHeight / 2));
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


        public void Draw()
        {
            Map.Draw(Globals.spriteBatch);
            for (int i = 0; i < GameObjects.Count; i++)
                GameObjects[i].Draw();            

        }


        #region Load

        private void LoadMap(string fileName)
        {
            Portals = new List<Portal>();
            Level.ClipMap = new Dictionary<Vector2, Rectangle>();
            Level.Enemies = new List<Enemy>();
            if (Level.Player != null)
            {
                Level.Player = (Player)GameObjects[0];
                GameObjects.Clear();
                GameObjects.Add(Level.Player);
            }
            Map = Globals.content.Load<Map>(fileName);

            MapObjectLayer objects = Map.GetLayer("Objects") as MapObjectLayer;

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
                    case "Portal2":
                    case "Portal3":
                    case "Portal4":
                        Portal portal = new Portal();
                        portal.Bounds = obj.Bounds;
                        Property tempP = obj.Properties["DestinationMap"];
                        portal.DestinationMap = tempP.RawValue;
                        //portal.DestinationMap = (string)obj.Properties["DestinationMap"];
                        tempP = obj.Properties["DestinationX"];
                        string tileLocX = tempP.RawValue; //obj.Properties["DestinationX"].RawValue;
                        string tileLocY = obj.Properties["DestinationY"].RawValue;
                        portal.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLocX), Convert.ToInt32(tileLocY));
                        Portals.Add(portal);
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
                        Portals.Add(crys);
                        break;
                }
            }
            LoadClipMap();
            LoadEnemies();
            LoadBoss();
        }

        private void LoadClipMap()
        {
            ClipLayer = new Dictionary<Vector2, Rectangle>();
            TileLayer clipLayer = Map.GetLayer("Clip") as TileLayer;            
            for (int y = 0; y < clipLayer.Width; y++)
                for (int x = 0; x < clipLayer.Height; x++)
                {
                    Tile tile = clipLayer.Tiles[x, y];
                    if (tile != null)
                        ClipLayer.Add(new Vector2(x, y), new Rectangle(x * tile.Source.Width, y * tile.Source.Height, tile.Source.Width, tile.Source.Height));
                }
            Map.GetLayer("Clip").Visible = false; 
        }

        private void LoadEnemies()
        {
            Level.Enemies = new List<Enemy>();
            TileLayer enemyLayer = Map.GetLayer("Enemy") as TileLayer;
            for (int y = 0; y < enemyLayer.Width; y++)
                for (int x = 0; x < enemyLayer.Height; x++)
                {
                    Tile tile = enemyLayer.Tiles[x, y];
                    if (tile != null)
                    {
                        string enemyType = tile.Properties["Type"].RawValue;
                        int enemyX = x * Map.TileWidth;
                        int enemyY = y * Map.TileHeight;
                        if (enemyType == "Fire")
                            GameObjects.Add(new FireEnemy(new Vector2(enemyX, enemyY)));                                          
                    }
                }
            Map.GetLayer("Enemy").Visible = false; 
        }

        private void LoadBoss()
        {
            TileLayer bossLayer = Map.GetLayer("Boss") as TileLayer;
            for (int y = 0; y < bossLayer.Width; y++)
                for (int x = 0; x < bossLayer.Height; x++)
                {
                    Tile tile = bossLayer.Tiles[x, y];
                    if (tile != null)
                    {
                        string bossType = tile.Properties["Type"].RawValue;
                        int bossX = x * Map.TileWidth;
                        int bossY = y * Map.TileHeight;
                        if (bossType == "Fire")
                            GameObjects.Add(new FireBoss(new Vector2(bossX, bossY)));
                    }
                }
            Map.GetLayer("Boss").Visible = false;
        }

        #endregion //Load



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


    }
}
