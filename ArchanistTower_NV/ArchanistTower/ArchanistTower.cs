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
            //shader.DrawSetup();
            GraphicsDevice.Clear(Color.Black);
            Globals.screenManager.Draw(gameTime);       
        }

        public static void ExitGame()
        {
            Quit = true;
        }
    }
}
