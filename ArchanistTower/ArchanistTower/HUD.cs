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

            Globals.spriteBatch.DrawString(Globals.spriteFont, time, AdjustForCamera(new Vector2(400, 0)), Color.White);
            Globals.spriteBatch.Draw(lifeBarTexture, AdjustForCamera(lifeBar), Color.White);
            Globals.spriteBatch.Draw(fireBallTexture, AdjustForCamera(new Rectangle(400, 400, 64, 64)), Color.White);
        }

        private Rectangle AdjustForCamera(Rectangle rect)
        {
            return new Rectangle(rect.X + (int)camera.Position.X, rect.Y + (int)camera.Position.Y, rect.Width, rect.Height);
        }
        private Vector2 AdjustForCamera(Vector2 vect)
        {
            return vect+camera.Position;
        }
    }
}
