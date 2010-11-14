using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArchanistTower.Screens
{
    class GameOverScreen : Screen
    {
        SpriteFont Font;
        Texture2D image;
        Rectangle imageRectangle;

        public GameOverScreen()
        {
            image = Globals.content.Load<Texture2D>("Menuscreens\\gameover");
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Initialize();
        }
        
        protected override void Initialize()
        { 
            Font = Globals.content.Load<SpriteFont>("Fonts\\Arial");
        }

        protected override void Unload()
        { }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Globals.input.KeyJustPressed(Keys.Enter) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.A))
            {
                Globals.screenManager.AddScreen(new MenuScreen());
                Globals.screenManager.RemoveScreen("GameScreen");
                Globals.screenManager.RemoveScreen("HUDScreen");
                this.Destroy();
            }
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(image, imageRectangle, Color.Gray);
            Globals.spriteBatch.End();
        }
    }
}
