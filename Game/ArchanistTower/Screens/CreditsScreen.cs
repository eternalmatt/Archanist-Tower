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
    class CreditsScreen : Screen
    {
        Texture2D image;
        Rectangle imageRectangle;
        int y;

        public CreditsScreen()
        {
            y = 0;
            image = Globals.content.Load<Texture2D>("Menuscreens\\credits");
            imageRectangle = new Rectangle(0, y, image.Width, image.Height);
            Initialize();
        }

        protected override void Initialize()
        {
            Name = "CreditsScreen";
        }

        protected override void Unload()
        {   }

        protected override void Update(GameTime gameTime)
        {
#if WINDOWS
            if(Globals.input.KeyJustPressed(Keys.Enter) ||
                Globals.input.KeyJustPressed(Keys.Escape))
            {
                this.Destroy();
                Globals.screenManager.AddScreen(new MenuScreen());
            }
#endif
            if(Globals.input.ButtonPressed(PlayerIndex.One, Buttons.A) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.Start))
            {
                this.Destroy();
                Globals.screenManager.AddScreen(new MenuScreen());
            }

            y -= 1;
            imageRectangle.Y = y;

            if ((imageRectangle.Y) * -1 > (image.Height - Globals.ScreenHeight))
            {
                this.Destroy();
                Globals.screenManager.AddScreen(new MenuScreen());
            }
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(image, imageRectangle, Color.White);
            Globals.spriteBatch.End();
        }
    }
}
