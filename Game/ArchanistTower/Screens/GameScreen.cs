using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArchanistTower.GameObjects;
using Microsoft.Xna.Framework.Input;
using TiledLib;

namespace ArchanistTower.Screens
{
    public class GameScreen : Screen
    { 
        public static GameWorld gameWorld;
        ShaderCode shader = new ShaderCode();
        HUDScreen HUD;
         
        public GameScreen() 
        {
            gameWorld = new GameWorld();
            Initialize();
            shader.LoadContent();
        }

        protected override void Initialize()
        {
            Name = "GameScreen";
            HUD = new HUDScreen();
            gameWorld.Initialize();

            shader.Initialize();
            
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
            shader.Update();
            gameWorld.Update(gameTime);
        }

        protected override void Draw()
        {
            shader.DrawSetup();
            Globals.spriteBatch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Deferred,
                SaveStateMode.None,
                Globals.camera.TransformMatrix);
            gameWorld.Draw();
            Globals.spriteBatch.End();
            shader.Draw();
        }


        public override void OnAdd()
        {
            Globals.screenManager.AddScreen(HUD);
        }

        public static void GameOver()
        {
            Globals.screenManager.AddScreen(new GameOverScreen());
            Globals.screenManager.FindScreen("GameScreen").Disable();
        }
        
    }
}
