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
        protected int currentLevel;
        ShaderCode shader = new ShaderCode();

        public GameWorld()
        {
            gameObjects = new List<GameObject>();
            shader.Initialize();
            shader.LoadContent();
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
            shader.DrawSetup();
            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].Draw();
            shader.Draw();
        }

    }
}
