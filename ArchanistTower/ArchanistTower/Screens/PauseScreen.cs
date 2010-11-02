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
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRectangle;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        float FadeValue;
        float FadeSpeed = 60.0f;

        public int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        bool isActive = false;

        public bool BackToMenu
        {
            get { return backToMenu; }
            set { backToMenu = value; }
        }

        bool backToMenu = false;

        public PauseScreen(Game game, Texture2D image)
            : base(game, Globals.spriteBatch)
        {
            string[] menuItems = { "Resume Game", "Exit To Main Menu" };
            menuComponent = new MenuComponent(game, menuItems);
            Components.Add(menuComponent);
            this.image = image;
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (FadeValue < 8)
            {
                FadeValue = FadeValue + (timeDelta * FadeSpeed);
            }
            else
            {
                FadeValue = 8;
            }

            keyboardState = Keyboard.GetState();
            if (CheckKey(Keys.Enter) || CheckKey(Keys.Space))
            {
                this.IsActive = false;
                FadeValue = 0;
                if (menuComponent.SelectedIndex == 1)
                {
                    this.BackToMenu = true;
                }
                // sleep for 100ms so that it won't jump straight back to game after exiting
                // can't think of a better solution right now
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