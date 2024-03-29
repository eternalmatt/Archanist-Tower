﻿using System;
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

        float FadeValue;
        float FadeSpeed = 60.0f;

        public GameOverScreen()
        {
            image = Globals.content.Load<Texture2D>("Menuscreens\\gameover");
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Initialize();
        }
        
        protected override void Initialize()
        { 
            Font = Globals.content.Load<SpriteFont>("Fonts\\Arial");
            Name = "GameOverScreen";
        }

        protected override void Unload()
        { }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
#if WINDOWS
            if (Globals.input.KeyJustPressed(Keys.Enter))
            {
                Globals.screenManager.AddScreen(new CreditsScreen()); // return to splash screen after game over
                Globals.screenManager.FindScreen("GameScreen").Destroy(); // destroy any existing game screen
                Globals.screenManager.FindScreen("HUDScreen").Destroy(); // and hud screen
                GameWorld.Player = null; // remove the player to prepare for a new game
                this.Destroy(); // destroy this game over screen
            }
#endif

            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.A) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.Start))
            {
                Globals.screenManager.AddScreen(new CreditsScreen()); // return to splash screen after game over
                Globals.screenManager.FindScreen("GameScreen").Destroy(); // destroy any existing game screen
                Globals.screenManager.FindScreen("HUDScreen").Destroy(); // and hud screen
                GameWorld.Player = null; // remove the player to prepare for a new game
                this.Destroy(); // destroy this game over screen
            }

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FadeValue < 255)
            {
                FadeValue = FadeValue + (timeDelta * FadeSpeed); // calculates the alpha value (transparency) and set it as FadeValue
            }
            else
            {
                FadeValue = 255;
            }     
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            //Globals.spriteBatch.Draw(image, imageRectangle, Color.Gray);
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
