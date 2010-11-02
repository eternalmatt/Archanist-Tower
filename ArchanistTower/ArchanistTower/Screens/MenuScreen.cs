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
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRectangle;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        float FadeValue;
        float FadeSpeed = 20.0f;

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

        public MenuScreen(Game game, Texture2D image)
            : base(game, Globals.spriteBatch)
        {
            string[] menuItems = { "Start Game", "Exit" };
            menuComponent = new MenuComponent(game, menuItems);
            Components.Add(menuComponent);
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
                FadeValue = 0;
                if (menuComponent.SelectedIndex == 0)
                {
                    this.IsActive = false;
                }
                else
                {
                    Game.Exit();
                }
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