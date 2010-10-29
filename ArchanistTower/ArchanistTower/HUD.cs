using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower
{
    public class HUD
    {
        Texture2D lifeBarTexture;
        public Rectangle lifeBar = new Rectangle(200,0,0,0);

        public void LoadContent()
        {
            lifeBarTexture = Globals.content.Load<Texture2D>("HUD/rectangle");
        }


        public void Draw(GameTime gameTime)
        {   
            //get the time and display M:SS.mm
            String time = gameTime.TotalGameTime.ToString().Substring(4,7);

            Globals.spriteBatch.DrawString(Globals.spriteFont, time, new Vector2(400, 0), Color.White);
            Globals.spriteBatch.Draw(lifeBarTexture, lifeBar, Color.White);
        }



    }
}
