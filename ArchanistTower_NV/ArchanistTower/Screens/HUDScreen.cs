using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ArchanistTower.Screens
{
    class HUDScreen : Screen
    {
        SpriteFont Font;
        int PlayerHealth;

        public HUDScreen()
        {

            Initialize();
        }

        protected override void Unload()
        {
            throw new NotImplementedException();
        }
        
        protected override void Initialize()
        {
            Name = "HUDScreen";
            Font = Globals.content.Load<SpriteFont>("Fonts\\menufont");
        }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            PlayerHealth = 3;

        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.DrawString(Font, "Health:  " + PlayerHealth.ToString(), new Vector2(5, 5), Color.AntiqueWhite);
            Globals.spriteBatch.End(); 
        }
    }
}
