using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArchanistTower.GameObjects;
using Microsoft.Xna.Framework.Input;

namespace ArchanistTower.Screens
{
    public class GameScreen : Screen
    { 
        public static GameWorld gameWorld;
        public static Level gameMap;
         
        public GameScreen() 
        {
            gameWorld = new GameWorld();
            Initialize();
        }

        protected override void Initialize()
        {
            Name = "GameScreen";
            gameMap = new Level("Levels\\TestMap\\TestMap", new Player());
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {
            if (Globals.input.KeyJustPressed(Keys.Escape))
            {
                this.Disable();
                Globals.screenManager.AddScreen(new PauseScreen());
            }

            gameWorld.Update(gameTime);
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            gameMap.Draw();
            gameWorld.Draw();
            Globals.spriteBatch.End();
        }

    }
}
