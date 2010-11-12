using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArchanistTower.GameObjects;
using TiledLib;

namespace ArchanistTower
{
    public class GameWorld
    {
        Map map;
        public List<GameObject> gameObjects;
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
            gameObjects = new List<GameObject>();
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
            gameObjects.Add(obj);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
                if (gameObjects[i] is Player)
                {
                    foreach (Rectangle clip in Level.ClipMap.Values)
                        if (gameObjects[i].SpriteAnimation.Bounds.Intersects(clip))
                        {
                            gameObjects[i].Collision();
                            break;
                        }
                    foreach (Portal portal in Level.Portals)
                    {
                        if (gameObjects[i].SpriteAnimation.Bounds.Intersects(portal.Bounds))
                        {
                            LoadMap(portal.DestinationMap);
                            gameObjects[i].SpriteAnimation.Position = new Vector2(
                                (portal.DestinationTileLocation.X * map.TileWidth) + (map.TileWidth / 2),
                                (portal.DestinationTileLocation.X * map.TileHeight) + (map.TileHeight / 2));
                        }
                    }
                }
                       
                //if (gameObjects[i].Colldable)
                 //   for (int j = 0; j < gameObjects.Count; j++)
                 //       if (i != j)
                 //           gameObjects[i].Collision(gameObjects[j]);
                
                if (gameObjects[i].Dead)
                    gameObjects.RemoveAt(i);
            }
            
        }

        public void Draw()
        {
            shader.DrawSetup();
            map.Draw(Globals.spriteBatch);
            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].Draw();            
            shader.Draw();
        }

        #region Load

        private void LoadMap(string fileName)
        {
            Level.Portals = new List<Portal>();
            Level.ClipMap = new Dictionary<Vector2, Rectangle>();
            Level.Enemies = new List<Enemy>();

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
                            gameObjects.Add((Player)Level.Player);
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
            /*
            Level.Enemies = new List<Enemy>();
            TileLayer enemyLayer = map.GetLayer("Enemies") as TileLayer;
            for (int y = 0; y < enemyLayer.Width; y++)
                for (int x = 0; x < enemyLayer.Height; x++)
                {
                    Tile tile = enemyLayer.Tiles[x, y];
                    if (tile != null)
                    {
                        /*
                        Monster monster = new Monster(new Vector2((x * map.TileWidth) + (map.TileWidth / 2), y * map.TileHeight), new Vector2(138f, 192f), 6);
                        monster.AssetName = GetMonsterAssetName(tile.GetTileIndex());
                        monster.Type = GetMonsterType(tile.GetTileIndex());
                        monster.LoadContent(ScreenManager.Game.Content, monster.AssetName);
                        monster.UpdateBounds(map.TileWidth, map.TileHeight);
                        monster.Map = map;
                        World.Monsters.Add(monster);
                        
                    }
                }
            */
        }

        #endregion //Load

    }
}
