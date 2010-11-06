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
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string[] menuItems; //Menu List
        int selectedIndex; //List item currently selected

        //color of selected and non-selected items
        Color normal = Color.White;
        Color highLight = Color.Orange;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        Vector2 position;
        float width = 0.0f;
        float height = 0.0f;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                if(selectedIndex < 0)
                    selectedIndex = 0;
                if(selectedIndex >= menuItems.Length)
                    selectedIndex = menuItems.Length - 1;
            }
        }

        public MenuComponent(Game game, string[] menuItems)
            : base(game)
        {

            this.menuItems = menuItems;
            MeasureMenu();
        }

        private void MeasureMenu()
        {
            height = 0;
            width = 0;
            foreach (string item in menuItems)
            {
                Vector2 size = Globals.menuFont.MeasureString(item);
                if (size.X > width)
                    width = size.X;
                height += Globals.menuFont.LineSpacing + 10;
            }

            position = new Vector2(Globals.ScreenMiddleX - width / 2, Globals.ScreenMiddleY);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private bool CheckKey(Keys key)
        {
            return keyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyDown(key);
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if(CheckKey(Keys.Down))
            {
                selectedIndex++;
                if(selectedIndex == menuItems.Length)
                    selectedIndex = menuItems.Length - 1;
            }
            if(CheckKey(Keys.Up))
            {
                selectedIndex--;
                if(selectedIndex < 0)
                    selectedIndex = 0;
            }
            base.Update(gameTime);

            oldKeyboardState = keyboardState;
        }

        public override void  Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Vector2 location = position;
            Color tint;

            Globals.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            for(int i = 0; i < menuItems.Length; i++)
            {
                if(i == selectedIndex)
                    tint = highLight;
                else
                    tint = normal;
                Globals.spriteBatch.DrawString(
                    Globals.menuFont,
                    menuItems[i],
                    location,
                    tint);
                location.Y += Globals.spriteFont.LineSpacing + 10;
            }
            Globals.spriteBatch.End();
        }
    }
}