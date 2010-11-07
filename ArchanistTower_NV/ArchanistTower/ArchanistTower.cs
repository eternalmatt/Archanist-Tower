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
       // Map map;
        //Level l = new Level();
        Player player;
        HUD hud = new HUD();

        Screens.MenuScreen menuscreen;
        Screens.PauseScreen pausescreen;
        Screens.SplashScreen splashscreen;
        Texture2D menubg;
        Texture2D pausebg;
        Texture2D splashbg;

        public ArchanistTower()
        {
            Globals.Initialize(this);
            player = new Player();
        }
        protected override void Initialize()
        {
            player.Initialize();
            base.Initialize();            
        }

        protected override void LoadContent()
        {
            Globals.LoadContent();
            player.LoadContent();
            hud.LoadContent(); 
            //map = Globals.content.Load<Map>("Levels/TestMap/TestMap");
            //l.AddMap(map);
            //l.CurrentMap = 2;

            menubg = Globals.content.Load<Texture2D>("Menuscreens/menu");
            menuscreen = new Screens.MenuScreen(this, menubg);
            pausebg = Globals.content.Load<Texture2D>("Menuscreens/pause");
            pausescreen = new Screens.PauseScreen(this, pausebg);
            splashbg = Globals.content.Load<Texture2D>("Menuscreens/splash");
            splashscreen = new Screens.SplashScreen(this, splashbg);
        }

  
        protected override void UnloadContent()
        {  }

        protected override void Update(GameTime gameTime)
        {
            if (splashscreen.IsActive)
            {
                splashscreen.Update(gameTime);
                if (splashscreen.AdvanceToMenu == true)
                { 
                    menuscreen.IsActive = true; 
                }
            }
            else if (menuscreen.IsActive)
            {
                menuscreen.Update(gameTime);
            }
            else if (pausescreen.IsActive)
            {
                pausescreen.Update(gameTime);
                if (pausescreen.BackToMenu)
                {
                    menuscreen.IsActive = true;
                    pausescreen.BackToMenu = false;
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    pausescreen.IsActive = true;
                }

                //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                //    this.Exit();
                //l.Update(gameTime);
                hud.PlayerLifeBar++;
                player.Update(gameTime);
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (splashscreen.IsActive)
            {
                splashscreen.Draw(gameTime);
            }
            else if (menuscreen.IsActive)
            {
                menuscreen.Draw(gameTime);
            }
            else if (pausescreen.IsActive)
            {
                pausescreen.Draw(gameTime);
            }
            else
            {
                //Globals.GraphicsDevice.Clear(Color.CornflowerBlue);

                player.Draw();

                Globals.spriteBatch.Begin();
                hud.Draw(gameTime);
                Globals.spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
