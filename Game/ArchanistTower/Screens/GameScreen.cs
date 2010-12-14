using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArchanistTower.GameObjects;
using Microsoft.Xna.Framework.Input;
using TiledLib;
using Microsoft.Xna.Framework.Media;

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
            Globals.ResetColor();
            Globals.BGSong = Globals.content.Load<Song>("Sounds\\Overworld_Theme");
            MediaPlayer.Play(Globals.BGSong);
           // MediaPlayer.Pause();
           // MediaPlayer.Resume();
            MediaPlayer.Volume = Globals.BGVolume();
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {
#if WINDOWS
            if (Globals.input.KeyJustPressed(Keys.Escape))
            {
                this.Disable(); // disable game screen when Esc is pressed
                Globals.screenManager.AddScreen(new PauseScreen()); // then add a pause screen
                MediaPlayer.Pause();
            }
#endif
            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.Start))
            {
                this.Disable(); // disable game screen when Esc is pressed
                Globals.screenManager.AddScreen(new PauseScreen()); // then add a pause screen
                MediaPlayer.Pause();
            }
            shader.Update();
            gameWorld.Update(gameTime);
            //MediaPlayer.Volume = Globals.BGVolume();
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
            //shader.Draw();
        }


        public override void OnAdd()
        {
            Globals.screenManager.AddScreen(HUD); // add a HUD
        }

        public static void GameOver()
        {
            Globals.screenManager.AddScreen(new GameOverScreen()); // create a game over screen
            Globals.screenManager.FindScreen("GameScreen").Disable(); // and disable this game screen
        }
        
    }
}
