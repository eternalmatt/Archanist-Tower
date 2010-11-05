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
            hud.LoadCamera(Globals.camera);
            //map = Globals.content.Load<Map>("Levels/TestMap/TestMap");
            //l.AddMap(map);
            //l.CurrentMap = 2;
        }

  
        protected override void UnloadContent()
        {  }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //l.Update(gameTime);
            hud.PlayerLifeBar++;
            player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Globals.GraphicsDevice.Clear(Color.CornflowerBlue);

            player.Draw();

            Globals.spriteBatch.Begin();
            hud.Draw(gameTime);
            Globals.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
