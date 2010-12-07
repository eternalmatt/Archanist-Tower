using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ArchanistTower.Screens
{
    class StoryScreen : Screen
    {
        Texture2D frame1, frame2, frame3;
        String f1, f2, f3, buttonText;
        Rectangle imageRectangle;
        int currentFrame;
        SpriteFont font;

        public StoryScreen()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            frame1 = Globals.content.Load<Texture2D>("Menuscreens\\Frame1");
            frame2 = Globals.content.Load<Texture2D>("Menuscreens\\Frame2");
            frame3 = Globals.content.Load<Texture2D>("Menuscreens\\Frame3");
            font = Globals.content.Load<SpriteFont>("Fonts\\Arial");
            
            f1 = "You are the powerful color wizard!";
            f2 = "The colors are stolen from the world which makes you powerless";
            f3 = "You must kill everyone and regain your powers by collecting the color relics.";
            buttonText = "Press Enter(Keyboard)/A(Controller)";

            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);

            Name = "StoryScreen";

            currentFrame = 1;
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {
            if (Globals.input.KeyJustPressed(Keys.Enter) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.A) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.Start))
            {
                currentFrame++;
            }

            if (currentFrame > 3)
            {
                this.Destroy();
                Globals.screenManager.AddScreen(new GameScreen());
            }
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            if (currentFrame == 1)
            {
                Globals.spriteBatch.Draw(frame1, imageRectangle, Color.White);
                Globals.spriteBatch.DrawString(font, f1, new Vector2(100, 50), Color.Red);
            }
            if (currentFrame == 2)
            {
                Globals.spriteBatch.Draw(frame2, imageRectangle, Color.White);
                Globals.spriteBatch.DrawString(font, f2, new Vector2(100, 50), Color.Red);
            }
            if (currentFrame == 3)
            {
                Globals.spriteBatch.Draw(frame3, imageRectangle, Color.White);
                Globals.spriteBatch.DrawString(font, f3, new Vector2(100, 50), Color.Red);
            }

            Globals.spriteBatch.DrawString(font, buttonText, new Vector2(480, 550), Color.Black);
            Globals.spriteBatch.End();
        }
    }
}
