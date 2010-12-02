using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArchanistTower.GameObjects;
using ArchanistTower.Collectables;
using TiledLib;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower
{
    public class GameWorld
    {

        //Map Things
        public static Map Map { get; set; }
        public static Dictionary<Vector2, Rectangle> ClipMap { get; set; }
        public static List<Portal> Portals { get; set; }
        public static List<Enemy> Enemies { get; set; }
        public static List<Spell> Spells { get; set; }
        public static List<Collectable> Collectables { get; set; }
        public static bool Debug { get; set; }

        public static Player Player;


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
           // Portals = new List<Portal>();
            Enemies = new List<Enemy>();
            Spells = new List<Spell>();
            Collectables = new List<Collectable>();
            //shader.LoadContent();
        }

        public void Initialize()
        {
            //shader.Initialize();
            LoadMap("Levels//TestFireMap//MountainEntrance");
            Debug = false;

        }

        public void Update(GameTime gameTime)
        {
            PlayerUpdate(gameTime);
            EnemyUpdate(gameTime);
            SpellUpdate(gameTime);

            foreach (Collectable c in Collectables)
                c.Update(gameTime);
        }

        private void SpellUpdate(GameTime gameTime)
        {
            for(int i = 0; i < Spells.Count; i++)
            {
                Spells[i].Update(gameTime);
                foreach (Rectangle c in ClipMap.Values)
                    if (Spells[i].SpriteAnimation.Bounds.Intersects(c))
                        Spells[i].WorldCollision();

                foreach (Enemy e in Enemies)
                    if (Math.Abs(Spells[i].SpriteAnimation.Position.X - e.SpriteAnimation.Position.X) <= Spells[i].CollisionRadius &&
                        Math.Abs(Spells[i].SpriteAnimation.Position.Y - e.SpriteAnimation.Position.Y) <= Spells[i].CollisionRadius)
                        if (PerPixelCollision(Spells[i].SpriteAnimation.Bounds, Spells[i].SpriteAnimation.SpriteTexture,
                            e.SpriteAnimation.Bounds, e.SpriteAnimation.SpriteTexture))
                            Spells[i].Collision(e);

                if (Spells[i].Dead)
                    Spells.RemoveAt(i);
            }
        }

        private void EnemyUpdate(GameTime gameTime)
        {
            foreach (Enemy e in Enemies)
            {
                e.Update(gameTime);
                foreach (Rectangle c in ClipMap.Values)
                    if (e.SpriteAnimation.Bounds.Intersects(c))
                        e.WorldCollision();
                e.PlayerPosition = Player.SpriteAnimation.Position;

                if (e is FireBoss)
                {
                    e.spellMotionList.Clear();
                    e.spellPositionList.Clear();
                    foreach (Spell spell in Spells)
                        if (spell.originatingType == "Player")
                        {
                            e.spellMotionList.Add(spell.motion);
                            e.spellPositionList.Add(spell.SpriteAnimation.Position);
                        }
                }
            }
            for (int i = 0; i < Enemies.Count; i++)
                if (Enemies[i].Dead) Enemies.RemoveAt(i--);
        }

        private void PlayerUpdate(GameTime gameTime)
        {
            Player.Update(gameTime);

            foreach (Rectangle c in ClipMap.Values)
                if (Player.SpriteAnimation.Bounds.Intersects(c))
                    Player.WorldCollision();

            foreach (Portal p in Portals)
                if (Player.SpriteAnimation.Bounds.Intersects(p.Bounds))
                {
                    LoadMap(p.DestinationMap);
                    Player.SpriteAnimation.Position = new Vector2(
                        (p.DestinationTileLocation.X * Map.TileWidth) + (Map.TileWidth / 2),
                        (p.DestinationTileLocation.Y * Map.TileHeight) + (Map.TileHeight / 2));
                }

            foreach (Collectable c in Collectables)
                if (Math.Abs(Player.SpriteAnimation.Position.X - c.SpriteAnimation.Position.X) <= Player.CollisionRadius &&
                    Math.Abs(Player.SpriteAnimation.Position.Y - c.SpriteAnimation.Position.Y) <= Player.CollisionRadius)
                    if (PerPixelCollision(Player.SpriteAnimation.Bounds, Player.SpriteAnimation.SpriteTexture,
                        c.SpriteAnimation.Bounds, c.SpriteAnimation.SpriteTexture))
                        c.Collected();

            foreach (Enemy e in Enemies)
                if(!e.Dead)
                    if(Math.Abs(Player.SpriteAnimation.Position.X - e.SpriteAnimation.Position.X) <= Player.CollisionRadius &&
                        Math.Abs(Player.SpriteAnimation.Position.Y - e.SpriteAnimation.Position.Y) <= Player.CollisionRadius)
                        if (PerPixelCollision(Player.SpriteAnimation.Bounds, Player.SpriteAnimation.SpriteTexture,
                            e.SpriteAnimation.Bounds, e.SpriteAnimation.SpriteTexture))
                        {
                            Player.Collision(e);
                            e.Collision(Player);
                        }
        }
    



        public void Draw()
        {
            Map.Draw(Globals.spriteBatch);
            foreach(Enemy enemy in Enemies)
                enemy.Draw();
            foreach (Collectable c in Collectables)
                c.Draw();
            foreach (Spell s in Spells)
                s.Draw();
            Player.Draw();
        }




        #region Load

        private void LoadMap(string fileName)
        {
            Portals = new List<Portal>();
            Enemies = new List<Enemy>();
            Map = Globals.content.Load<Map>(fileName);

            MapObjectLayer objects = Map.GetLayer("Objects") as MapObjectLayer;

            foreach (MapObject obj in objects.Objects)
            {
                switch (obj.Name)
                {
                    case "PlayerStart":
                        if (Player == null)
                            Player = new Player(new Vector2(obj.Bounds.X, obj.Bounds.Y));
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
            //LoadBoss();
            LoadCollectables();
        }

        private void LoadClipMap()
        {
            ClipMap = new Dictionary<Vector2, Rectangle>();
            TileLayer clipLayer = Map.GetLayer("Clip") as TileLayer;            
            for (int y = 0; y < clipLayer.Width; y++)
                for (int x = 0; x < clipLayer.Height; x++)
                {
                    Tile tile = clipLayer.Tiles[x, y];
                    if (tile != null)
                        ClipMap.Add(new Vector2(x, y), new Rectangle(x * tile.Source.Width, y * tile.Source.Height, tile.Source.Width, tile.Source.Height));
                }
            Map.GetLayer("Clip").Visible = false;
        }

        private void LoadEnemies()
        {
            Enemies = new List<Enemy>();
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
                            Enemies.Add(new FireEnemy(new Vector2(enemyX, enemyY)));                                          
                    }
                }
            Map.GetLayer("Enemy").Visible = false;
        }

        //private void LoadBoss()
        //{
        //    TileLayer bossLayer = Map.GetLayer("Boss") as TileLayer;
        //    for (int y = 0; y < bossLayer.Width; y++)
        //        for (int x = 0; x < bossLayer.Height; x++)
        //        {
        //            Tile tile = bossLayer.Tiles[x, y];
        //            if (tile != null)
        //            {
        //                string bossType = tile.Properties["Type"].RawValue;
        //                int bossX = x * Map.TileWidth;
        //                int bossY = y * Map.TileHeight;
        //                if (bossType == "Fire")
        //                    Enemies.Add(new FireBoss(new Vector2(bossX, bossY)));
        //            }
        //        }
        //    Map.GetLayer("Boss").Visible = false;
        //}

        private void LoadCollectables()
        {
            TileLayer cLayer = Map.GetLayer("Collectable") as TileLayer;
            for (int y = 0; y < cLayer.Width; y++)
                for (int x = 0; x < cLayer.Height; x++)
                {
                    Tile tile = cLayer.Tiles[x, y];
                    if (tile != null)
                    {
                        string cType = tile.Properties["Type"].RawValue;
                        int cX = x * Map.TileWidth;
                        int cY = y * Map.TileHeight;
                        if (cType == "Health")
                            break;
                        else
                            Collectables.Add(new Crystal(cType, new Vector2(cX, cY)));
                    }
                }

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
