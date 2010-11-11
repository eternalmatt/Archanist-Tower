using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArchanistTower.GameObjects;

namespace ArchanistTower.Screens
{
    public class GameScreen : Screen
    { 
        public static GameWorld gameWorld;
         
        public GameScreen() 
        {
            gameWorld = new GameWorld();
        }

        protected override void Initialize()
        {
            gameWorld.AddObject(new Player());
            gameWorld.AddFirstLevel();
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {
            
            gameWorld.Update(gameTime);
        }

        protected override void Draw()
        {
            gameWorld.Draw();
        }

    }
}
