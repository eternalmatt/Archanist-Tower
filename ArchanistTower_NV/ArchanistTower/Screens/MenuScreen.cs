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
        MenuComponent menuComponent;
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
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {
            if (Globals.input.KeyJustPressed(Keys.Up))
                Selection--;
            if (Globals.input.KeyJustPressed(Keys.Down))
                Selection++;

            if (Selection < 0)
                Selection = 1;
            if (Selection > 1)
                Selection = 0;

            if (Globals.input.KeyJustPressed(Keys.Enter))
            {
                switch (Selection)
                {
                    case 0:
                        this.Destroy();
                        Globals.screenManager.AddScreen(new GameScreen());
                        break;
                    case 1:
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
                Globals.spriteBatch.DrawString(Font, "START THE GAME", new Vector2(540, 300), Color.Yellow);
                Globals.spriteBatch.DrawString(Font, "EXIT", new Vector2(540, 375), Color.White);
            }
            else if (Selection == 1)
            {
                Globals.spriteBatch.DrawString(Font, "START THE GAME", new Vector2(540, 300), Color.White);
                Globals.spriteBatch.DrawString(Font, "EXIT", new Vector2(540, 375), Color.Yellow);
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