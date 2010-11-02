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
    public class SplashScreen : Screen
    {
        Texture2D image;
        Rectangle imageRectangle;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        float FadeValue;
        float FadeSpeed = 10.0f;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        bool isActive = true;

        public bool AdvanceToMenu
        {
            get { return advanceToMenu; }
            set { advanceToMenu = value; }
        }

        bool advanceToMenu = false;

        public SplashScreen(Game game, Texture2D image)
            : base(game, Globals.spriteBatch)
        {
            this.image = image;
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FadeValue < 255)
            {
                FadeValue = FadeValue + (timeDelta * FadeSpeed);
            }
            else
            {
                FadeValue = 255;
            }

            keyboardState = Keyboard.GetState();
            if (CheckKey(Keys.Enter) || CheckKey(Keys.Space))
            {
                this.IsActive = false;
                this.AdvanceToMenu = true;
                FadeValue = 0;
                Thread.Sleep(100);
            }

            base.Update(gameTime);
            oldKeyboardState = keyboardState;
        }

        private bool CheckKey(Keys key)
        {
            return keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyDown(key);
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            Globals.spriteBatch.Draw(image, imageRectangle, FadeColor(Color.White, FadeValue));
            Globals.spriteBatch.End();
            base.Draw(gameTime);
        }

        public Color FadeColor(Color baseColor, float FadeValue)
        {
            Color tempColor;
            tempColor = new Color(baseColor.R, baseColor.G, baseColor.B, (byte)FadeValue);
            return tempColor;
        }
    }
}