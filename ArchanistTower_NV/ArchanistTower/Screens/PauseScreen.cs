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
    public class PauseScreen : Screen
    {
        SpriteFont Font;
        Texture2D image;
        Rectangle imageRectangle;

        float FadeValue;
        float FadeSpeed = 120.0f;

        int Selection;

        public PauseScreen()
        {
            image = Globals.content.Load<Texture2D>("MenuScreens\\pause");
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Initialize();
        }

        protected override void Initialize()
        {
            Font = Globals.content.Load<SpriteFont>("Fonts\\Arial");
            Selection = 0;
            Name = "PauseScreen";
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
                Selection = 2;
            if (Selection > 2)
                Selection = 0;

            if (Globals.input.KeyJustPressed(Keys.Enter))
            {
                switch(Selection)
                {
                    case 0:
                        this.Destroy();
                        Globals.screenManager.FindScreen("GameScreen").Activate();
                        Globals.screenManager.FindScreen("HUDScreen").Activate();
                        break;
                    case 1:
                        Globals.screenManager.AddScreen(new InstructionScreen());
                        this.Disable();
                        break;
                    case 2:
                        this.Destroy();
                        Globals.screenManager.FindScreen("GameScreen").Destroy();
                        Globals.screenManager.AddScreen(new MenuScreen());
                        break;
                }
            }

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FadeValue < 125)
            {
                FadeValue = FadeValue + (timeDelta * FadeSpeed);
            }
            else
            {
                FadeValue = 125;
            }            
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            Globals.spriteBatch.Draw(image, imageRectangle, FadeColor(Color.White, FadeValue));
            if (Selection == 0)
            {
                Globals.spriteBatch.DrawString(Font, "RETURN TO GAME", new Vector2(540, 300), Color.Yellow);
                Globals.spriteBatch.DrawString(Font, "CONTROLS", new Vector2(540, 350), Color.White);
                Globals.spriteBatch.DrawString(Font, "EXIT TO MENU", new Vector2(540, 400), Color.White);
            }
            else if (Selection == 1)
            {
                Globals.spriteBatch.DrawString(Font, "RETURN TO GAME", new Vector2(540, 300), Color.White);
                Globals.spriteBatch.DrawString(Font, "CONTROLS", new Vector2(540, 350), Color.Yellow);
                Globals.spriteBatch.DrawString(Font, "EXIT TO MENU", new Vector2(540, 400), Color.White);
            }
            else
            {
                Globals.spriteBatch.DrawString(Font, "RETURN TO GAME", new Vector2(540, 300), Color.White);
                Globals.spriteBatch.DrawString(Font, "CONTROLS", new Vector2(540, 350), Color.White);
                Globals.spriteBatch.DrawString(Font, "EXIT TO MENU", new Vector2(540, 400), Color.Yellow);
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