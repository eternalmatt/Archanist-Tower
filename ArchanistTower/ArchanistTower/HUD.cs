using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine;

namespace ArchanistTower
{
    public class HUD
    {
        const int MAX_LIFEBARSIZE = 100;
        Texture2D lifeBarTexture, fireBallTexture;
        Rectangle lifeBar = new Rectangle(200,0,0,20);
        static Camera camera;
        
        public int PlayerLifeBar
        {   //encapsulated lifeBar to reduce errors
            get { return lifeBar.Width; }
            set { if (value > MAX_LIFEBARSIZE)
                    lifeBar.Width = MAX_LIFEBARSIZE;
                  else if (value < 0)
                    lifeBar.Width = 0;
                  else lifeBar.Width = value; }
        }
        //allows lifebar to stay same position on screen while camera moves
        private Rectangle PlayerLifeBarRectangle
        { get { return new Rectangle(lifeBar.X + (int)camera.Position.X, lifeBar.Y + (int)camera.Position.Y, lifeBar.Width, lifeBar.Height); } }


        public void LoadContent()
        {
            lifeBarTexture = Globals.content.Load<Texture2D>("HUD/rectangle");
            fireBallTexture = Globals.content.Load<Texture2D>("HUD/fireball_small");
        }

        public void LoadCamera(Camera cam)
        {
            camera = cam;
        }


        public void Draw(GameTime gameTime)
        {   
            //get the time and convert to M:SS.mm
            String time = gameTime.TotalGameTime.ToString().Substring(4,7);

            Globals.spriteBatch.DrawString(Globals.spriteFont, time, new Vector2(400, 0) + camera.Position, Color.White);
            Globals.spriteBatch.Draw(lifeBarTexture, PlayerLifeBarRectangle, Color.White);
            //Globals.spriteBatch.Draw(fireBallTexture, new Rectangle(400, 400, 64, 64), Color.White);
        }



    }
}
