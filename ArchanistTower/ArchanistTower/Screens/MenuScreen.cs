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

        public int SelectedIndex
        {
            get { return menuComponent.SelectedIndex; }
            set { menuComponent.SelectedIndex = value; }
        }

        public MenuScreen(Game game, Texture2D image)
            : base(game, Globals.spriteBatch)
        {
            string[] menuItems = { "Start Game", "End Game" };
            menuComponent = new MenuComponent(game, menuItems);
            Components.Add(menuComponent);
            this.image = image;
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Globals.spriteBatch.Draw(image, imageRectangle, Color.White);
            base.Draw(gameTime);
        }
    }
}