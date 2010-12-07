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


namespace ArchanistTower.Screens
{
    public class MenuScreen : Screen
    {
        SpriteFont Font;
        Texture2D image;
        Rectangle imageRectangle;

        int Selection;

        float FadeValue;
        float FadeSpeed = 120.0f;


        public MenuScreen()
        {
            image = Globals.content.Load<Texture2D>("Menuscreens\\menu");
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Initialize();
        }

        protected override void Initialize()
        {
            Font = Globals.content.Load<SpriteFont>("Fonts\\menufont");
            Selection = 0;
            Name = "MenuScreen";
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {
            if (Globals.input.KeyJustPressed(Keys.Up) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickUp) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.DPadUp))
                Selection--;
            if (Globals.input.KeyJustPressed(Keys.Down) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickDown) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.DPadDown))
                Selection++;

            if (Selection < 0)
                Selection = 2;
            if (Selection > 2)
                Selection = 0;

            if (Globals.input.KeyJustPressed(Keys.Enter)||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.A))
            {
                switch (Selection)
                {
                    case 0:
                        this.Destroy();
                        //Globals.screenManager.AddScreen(new GameScreen());
                        Globals.screenManager.AddScreen(new StoryScreen());
                        break;
                    case 1:
                        this.Disable();
                        Globals.screenManager.AddScreen(new SettingsScreen());
                        break;
                    case 2:
                        ArchanistTower.ExitGame();
                        break;
                }
            }

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FadeValue < 255)
            {
                FadeValue = FadeValue + (timeDelta * FadeSpeed);
            }
            else
            {
                FadeValue = 255;
            }
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            Globals.spriteBatch.Draw(image, imageRectangle, FadeColor(Color.White, FadeValue));
            if (Selection == 0)
            {
                Globals.spriteBatch.DrawString(Font, "START THE GAME", new Vector2(500, 300), Color.Yellow);
                Globals.spriteBatch.DrawString(Font, "SETTINGS", new Vector2(500, 375), Color.White);
                Globals.spriteBatch.DrawString(Font, "EXIT", new Vector2(500, 450), Color.White);
            }
            else if (Selection == 1)
            {
                Globals.spriteBatch.DrawString(Font, "START THE GAME", new Vector2(500, 300), Color.White);
                Globals.spriteBatch.DrawString(Font, "SETTINGS", new Vector2(500, 375), Color.Yellow);
                Globals.spriteBatch.DrawString(Font, "EXIT", new Vector2(500, 450), Color.White);
            }
            else
            {
                Globals.spriteBatch.DrawString(Font, "START THE GAME", new Vector2(500, 300), Color.White);
                Globals.spriteBatch.DrawString(Font, "SETTINGS", new Vector2(500, 375), Color.White);
                Globals.spriteBatch.DrawString(Font, "EXIT", new Vector2(500, 450), Color.Yellow);
            }
            Globals.spriteBatch.End();
        }

        public Color FadeColor(Color baseColor, float FadeValue)
        {
            Color tempColor;
            tempColor = new Color(baseColor.R, baseColor.G, baseColor.B, (byte)FadeValue);
            return tempColor;
        }
    }
}