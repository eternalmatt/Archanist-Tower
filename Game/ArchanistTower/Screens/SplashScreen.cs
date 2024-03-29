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

        float FadeValue;
        float FadeSpeed = 120.0f;

        public SplashScreen()
        {
            image = Globals.content.Load<Texture2D>("Menuscreens\\splash");
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Initialize();
        }

        protected override void Initialize()
        {
            Name = "SplashScreen";
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {            
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FadeValue < 255)
            {
                FadeValue = FadeValue + (timeDelta * FadeSpeed); // calculates the alpha value (transparency) and set it as FadeValue
            }
            else
            {
                FadeValue = 255;
            }
#if WINDOWS
            if (Globals.input.KeyJustPressed(Keys.Enter))
            {
                this.Destroy();
                Globals.screenManager.AddScreen(new MenuScreen());
            }
#endif

            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.A) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.Start))
            {
                this.Destroy();
                Globals.screenManager.AddScreen(new MenuScreen());
            }

        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            Globals.spriteBatch.Draw(image, imageRectangle, FadeColor(Color.White, FadeValue));
            Globals.spriteBatch.End();
        }

        public Color FadeColor(Color baseColor, float FadeValue) // returns the current faded color based on the current faded value
        {
            Color tempColor;
            tempColor = new Color(baseColor.R, baseColor.G, baseColor.B, (byte)FadeValue);
            return tempColor;
        }
    }
}