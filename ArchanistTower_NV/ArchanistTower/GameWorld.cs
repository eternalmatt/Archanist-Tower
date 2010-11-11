﻿using System;
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
        ShaderCode shader = new ShaderCode();

        public Level CurrentLevel
        {
            get { return Levels[currentLevel]; }
        }

        public GameWorld()
        {
            Levels = new List<Level>();
            gameObjects = new List<GameObject>();
            shader.Initialize();
            shader.LoadContent();
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
            shader.DrawSetup();
            CurrentLevel.Draw(Globals.spriteBatch);

            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].Draw();
            shader.Draw();
        }

        public void AddFirstLevel()
        {
            Map m = Globals.content.Load<Map>("Levels\\TestMap\\TestMap");
            Level l = new Level();
            l.AddMap(m);
            Levels.Add(l);
        }
    }
}
