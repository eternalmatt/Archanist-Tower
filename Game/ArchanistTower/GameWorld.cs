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

        public static ShaderCode shader = new ShaderCode();
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
            shader.Initialize();
            shader.LoadContent();
        }

        public void Initialize()
        {
            //LoadMap("Levels//TestFireMap//MountainEntrance");
            LoadMap("Levels//TestFireMap//ForestEntrance");
            Debug = false;
        }

        #region Update

        public void Update(GameTime gameTime)
        {
            PlayerUpdate(gameTime);
            EnemyUpdate(gameTime);
            SpellUpdate(gameTime);
            shader.Update();
            foreach (Collectable c in Collectables)
            {
                c.Update(gameTime);
                if (c is Crystal)
                {
                    Globals.crysLoc = new Vector4(c.SpriteAnimation.Bounds.X, c.SpriteAnimation.Bounds.Y, 64, 64);
                    Globals.crysLoc.Normalize();
                }
            }
        }
         
        private void SpellUpdate(GameTime gameTime)
        {
            for(int i = 0; i < Spells.Count; i++)
            {
                Spells[i].Update(gameTime);
                foreach (Rectangle c in ClipMap.Values)
                    if (Spells[i].SpriteAnimation.Bounds.Intersects(c))
                        Spells[i].WorldCollision();
                /*
                if (!Spells[i].SpriteAnimation.Bounds.Intersects(new Rectangle(0,0,800,600)))
                    Spells[i].WorldCollision();
                */
                if (Spells[i].originatingType == GameObjects.Spell.OriginatingType.Player)
                    foreach (Enemy e in Enemies)
                        if (Vector2.Distance(Spells[i].SpriteAnimation.Position, e.SpriteAnimation.Position) <= Spells[i].CollisionRadius)
                            if (Spells[i].SpriteAnimation.Bounds.Intersects(e.SpriteAnimation.Bounds))
                                if (PerPixelCollision(e.SpriteAnimation, Spells[i].SpriteAnimation))
                                    Spells[i].Collision(e);

                if (Spells[i].originatingType == GameObjects.Spell.OriginatingType.Enemy)
                    if (Vector2.Distance(Spells[i].SpriteAnimation.Position, Player.SpriteAnimation.Position) <= Spells[i].CollisionRadius)
                        if (Spells[i].SpriteAnimation.Bounds.Intersects(Player.SpriteAnimation.Bounds))
                            if (PerPixelCollision(Player.SpriteAnimation, Spells[i].SpriteAnimation))
                                Spells[i].Collision(Player);


                if (Spells[i].Dead)
                    Spells.RemoveAt(i--);
            }
        }

        private void EnemyUpdate(GameTime gameTime)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update(gameTime);
                foreach (Rectangle c in ClipMap.Values)
                    if (Enemies[i].SpriteAnimation.Bounds.Intersects(c))
                        Enemies[i].WorldCollision();
                Enemies[i].PlayerPosition = Player.SpriteAnimation.Position;

                if (Enemies[i] is FireBoss)
                {   //if the enemy is a fireboss, we must repopulate spellLists with new information
                    Enemies[i].spellMotionList.Clear();
                    Enemies[i].spellPositionList.Clear();
                    foreach (Spell spell in Spells)
                        if (spell.originatingType == GameObjects.Spell.OriginatingType.Player)
                        {
                            Enemies[i].spellMotionList.Add(spell.motion);
                            Enemies[i].spellPositionList.Add(spell.SpriteAnimation.Position);
                        }
                }
                if (Enemies[i].Dead) Enemies.RemoveAt(i--);
            }               
        }

        private void PlayerUpdate(GameTime gameTime)
        {
            if (Player.Dead)
                Screens.GameScreen.GameOver();

            Player.Update(gameTime);

            foreach (Rectangle c in ClipMap.Values)
                if (Player.SpriteAnimation.Bounds.Intersects(c))
                    Player.WorldCollision();

            foreach (Portal p in Portals)
                if (Player.SpriteAnimation.Bounds.Intersects(p.Bounds))
                {
                    Spells.Clear();
                    LoadMap(p.DestinationMap);
                    Player.SpriteAnimation.Position = new Vector2(
                        (p.DestinationTileLocation.X * Map.TileWidth) + (Map.TileWidth / 2),
                        (p.DestinationTileLocation.Y * Map.TileHeight) + (Map.TileHeight / 2));
                }

            foreach (Collectable c in Collectables)
                if (Math.Abs(Player.SpriteAnimation.Position.X - c.SpriteAnimation.Position.X) <= Player.CollisionRadius &&
                    Math.Abs(Player.SpriteAnimation.Position.Y - c.SpriteAnimation.Position.Y) <= Player.CollisionRadius)
                    if (PerPixelCollision(c.SpriteAnimation, Player.SpriteAnimation))
                    {
                        c.Collected();
                        if (Player.selectedSpell == SelectedSpell.none) // the first time player collects a crystal
                        {
                            // automatically selects the spell for the player
                            if (Globals.red == 1) Player.selectedSpell = SelectedSpell.fire;
                            else if (Globals.green == 1) Player.selectedSpell = SelectedSpell.wind;
                            else Player.selectedSpell = SelectedSpell.water; 
                        }
                    }

            foreach (Enemy e in Enemies)
                if (!e.Dead)
                    if (Math.Abs(Player.SpriteAnimation.Position.X - e.SpriteAnimation.Position.X) <= Player.CollisionRadius &&
                        Math.Abs(Player.SpriteAnimation.Position.Y - e.SpriteAnimation.Position.Y) <= Player.CollisionRadius)
                        if (PerPixelCollision(e.SpriteAnimation, Player.SpriteAnimation))
                        {
                            Player.Collision(e);
                            e.Collision(Player);
                        }
        }

        #endregion //Update

        public void Draw()
        {
            shader.DrawSetup();
            Map.Draw(Globals.spriteBatch);
            foreach(Enemy enemy in Enemies)
                enemy.Draw();
            foreach (Spell s in Spells)
                s.Draw();
            Player.Draw();
            Globals.spriteBatch.End();
            shader.Draw();
            Globals.spriteBatch.Begin(
    SpriteBlendMode.AlphaBlend,
    SpriteSortMode.Deferred,
    SaveStateMode.None,
    Globals.camera.TransformMatrix);
            foreach (Collectable c in Collectables)
                c.Draw();
        }




        #region Load

        private void LoadMap(string fileName)
        {
            Portals = new List<Portal>();
            Enemies = new List<Enemy>();
            Spells = new List<Spell>();
            Map = Globals.content.Load<Map>(fileName);

            MapObjectLayer objects = Map.GetLayer("Objects") as MapObjectLayer;

            foreach (MapObject obj in objects.Objects)
            {
                switch (obj.Type)
                {
                    case "PlayerStart":
                        if (Player == null)
                            Player = new Player(new Vector2(obj.Bounds.X, obj.Bounds.Y));
                        break;

                    case "Portal":
                        Portal portal = new Portal();
                        portal.Bounds = obj.Bounds;
                        Property tempP = obj.Properties["DestinationMap"];
                        portal.DestinationMap = tempP.RawValue;
                        tempP = obj.Properties["DestinationX"];
                        string tileLocX = tempP.RawValue; 
                        string tileLocY = obj.Properties["DestinationY"].RawValue;
                        portal.DestinationTileLocation = new Vector2(Convert.ToInt32(tileLocX), Convert.ToInt32(tileLocY));
                        Portals.Add(portal);
                        break;                    
                }
            }
            LoadClipMap();
            LoadEnemies();
            LoadCollectables();
        }

        private void LoadClipMap()
        {
            ClipMap = new Dictionary<Vector2, Rectangle>();
            TileLayer clipLayer = Map.GetLayer("Clip") as TileLayer;            
            for (int x = 0; x < clipLayer.Width; x++)
                for (int y = 0; y < clipLayer.Height; y++)
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
            for (int x = 0; x < enemyLayer.Width; x++)
                for (int y = 0; y < enemyLayer.Height; y++)
                {
                    Tile tile = enemyLayer.Tiles[x, y];
                    if (tile != null)
                    {
                        string enemyType = tile.Properties["Type"].RawValue;
                        int enemyX = x * Map.TileWidth;
                        int enemyY = y * Map.TileHeight;
                        if (enemyType == "Fire")
                            Enemies.Add(new FireEnemy(new Vector2(enemyX, enemyY)));
                        if (enemyType == "Wind")
                            Enemies.Add(new WindEnemy(new Vector2(enemyX, enemyY)));
                        if (enemyType == "FireBoss")
                            Enemies.Add(new FireBoss(new Vector2(enemyX, enemyY)));
                    }
                }
            Map.GetLayer("Enemy").Visible = false;
        }

        private void LoadCollectables()
        {
            TileLayer cLayer = Map.GetLayer("Collectable") as TileLayer;
            for (int x = 0; x < cLayer.Width; x++)
                for (int y = 0; y < cLayer.Height; y++)
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
                        {
                            Collectables.Add(new Crystal(cType, new Vector2(cX, cY)));
                        }
                    }
                }
            Map.GetLayer("Collectable").Visible = false;
        }

        #endregion //Load

        #region PerPixelCollision
        /// <summary>
        /// Optimized PerPixelCollision
        /// </summary>
        private static bool PerPixelCollision(AnimatedSprite A, AnimatedSprite B)
        {
            int bottom = Math.Min(A.Bounds.Bottom, B.Bounds.Bottom);
            int right = Math.Min(A.Bounds.Right, B.Bounds.Right);

            for (int y = Math.Max(A.Bounds.Top, B.Bounds.Top); y < bottom; y++)
                for (int x = Math.Max(A.Bounds.Left, B.Bounds.Left); x < right; x++)
                    if (A.data[(x - A.Bounds.Left) + (y - A.Bounds.Top) * A.Bounds.Width].A != 0 &&
                        B.data[(x - B.Bounds.Left) + (y - B.Bounds.Top) * B.Bounds.Width].A != 0)
                        return true;
            return false;
        }
        /*
        /// <summary>
        /// Old PerPixelCollision
        /// </summary>
        private static bool PerPixelCollision(Rectangle rectangleA, Texture2D textureA, Rectangle rectangleB, Texture2D textureB)
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
            //I modified this section to sacrifice readability for better CPU by avoiding 
            //the repeated copying of large arrays of objects.
            // Get the color of both pixels at this point
            Color colorA = dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
            Color colorB = dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];
                    
            // If both pixels are not completely transparent,
            if (colorA.A != 0 && colorB.A != 0)
            {
                return true;// then an intersection has been found
            }
            
            return false;// No intersection found
        }
        */
        #endregion
    }
}
