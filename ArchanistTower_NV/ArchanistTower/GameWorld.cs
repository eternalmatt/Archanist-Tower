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

        public List<GameObject> gameObjects;
        public List<Level> Levels;
        protected int currentLevel;

        public Level CurrentLevel
        {
            get { return Levels[currentLevel]; }
        }

        public GameWorld()
        {
            Levels = new List<Level>();
            gameObjects = new List<GameObject>();
        }

        public void AddObject(GameObject obj)
        {
            gameObjects.Add(obj);
        }

        public void AddLevel(Level l)
        {
            Levels.Add(l);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
                if (gameObjects[i].Colldable)
                    for (int j = 0; j < gameObjects.Count; j++)
                        if (i != j)
                            gameObjects[i].Collision(gameObjects[j]);
                
                if (gameObjects[i].Dead)
                    gameObjects.RemoveAt(i);
            }

        }

        public void Draw()
        {
            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].Draw();
            CurrentLevel.Draw(Globals.spriteBatch);
        }

        public void AddFirstLevel()
        {
            Map m = Globals.content.Load<Map>("Levels\\TestMap\\TestMap");
        }
    }
}
