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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
       // Map map;
        //Level l = new Level();
        Player player;

        public Game1()
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
            player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Globals.GraphicsDevice.Clear(Color.CornflowerBlue);

            player.Draw();

            base.Draw(gameTime);
        }
    }
}
