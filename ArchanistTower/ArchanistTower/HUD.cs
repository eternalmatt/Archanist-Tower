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
        Texture2D blackRect;

        public void LoadContent()
        {
            blackRect = Globals.content.Load<Texture2D>("HUD/rectangle");
        }


        public void Draw(GameTime gameTime)
        {
            Globals.spriteBatch.DrawString(Globals.spriteFont, "Hello, HUD!", new Vector2(200, 200), Color.White);
            Globals.spriteBatch.Draw(blackRect, new Rectangle(0, 0, 100, 20), Color.White);
            Globals.spriteBatch.Draw(blackRect, new Rectangle(400, 400, 20, 20), Color.White);
            Globals.spriteBatch.Draw(blackRect, new Rectangle(450, 450, 100, 100), Color.White);

        }



    }
}
