using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using TiledLib;
using ArchanistTower.Screens;

namespace ArchanistTower
{
    public class ArchanistTower : Microsoft.Xna.Framework.Game
    {
        static bool Quit;

        private float _FPS = 0f, _TotalTime = 0f, _DisplayFPS = 0f;
        SpriteFont Font;

        public ArchanistTower()
        {
            Globals.Initialize(this);
            Components.Add(Globals.screenManager);
            //shader = new ShaderCode();
        }
        protected override void Initialize()
        {
            Quit = false;
            //shader.Initialize();
            base.Initialize();            
        }

        protected override void LoadContent()
        {
            Globals.LoadContent();
            Font = Globals.content.Load<SpriteFont>("Fonts\\Arial");
            //shader.LoadContent();

            Globals.screenManager.AddScreen(new SplashScreen());
        }

  
        protected override void UnloadContent()
        {  }

        protected override void Update(GameTime gameTime)
        {
            if (Quit)
                this.Exit();
            Globals.input.Update();            
            Globals.screenManager.Update(gameTime);            
        }

        protected override void Draw(GameTime gameTime)
        {
            // Calculate the Frames Per Second
            float ElapsedTime = (float)gameTime.ElapsedRealTime.TotalSeconds;
            _TotalTime += ElapsedTime;

            if (_TotalTime >= 1)
            {
                _DisplayFPS = _FPS;
                _FPS = 0;
                _TotalTime = 0;
            }
            _FPS += 1;

            // Format the string appropriately
            string FpsText = _DisplayFPS.ToString() + " FPS";
            Vector2 FPSPos = new Vector2((Globals.GraphicsDevice.Viewport.Width - Font.MeasureString(FpsText).X) - 15, 10);

            //shader.DrawSetup();
            GraphicsDevice.Clear(Color.Black);
            Globals.screenManager.Draw(gameTime);
           // Globals.spriteBatch.Begin();
           // Globals.spriteBatch.DrawString(Font, FpsText, FPSPos, Color.White);
           // Globals.spriteBatch.End();
        }

        public static void ExitGame()
        {
            Quit = true;
        }
    }
}
