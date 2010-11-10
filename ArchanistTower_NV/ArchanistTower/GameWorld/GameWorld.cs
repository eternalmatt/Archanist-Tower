using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.GameWorld
{
    class GameWorld
    {

        public List<GameObject> gameObjects;

        public GameWorld()
        { }

        public void Initialize()
        {
            gameObjects = new List<GameObject>();
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
        }
    }
}
