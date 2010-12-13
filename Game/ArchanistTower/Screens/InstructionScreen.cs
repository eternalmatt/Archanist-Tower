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
    public class InstructionScreen : Screen
    {
        Texture2D image;
        Rectangle imageRectangle;

        int Selection;

        public InstructionScreen()
        {
            image = Globals.content.Load<Texture2D>("Menuscreens\\instruction1");
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Initialize();
        }

        protected override void Initialize()
        {
            Selection = Globals.controlScheme;
            Name = "InstructionScreen";
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {
            // allow the user to select different control scheme using left / right controls
            if (Globals.input.KeyJustPressed(Keys.Left) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickLeft) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.DPadLeft))
                Selection--;
            if (Globals.input.KeyJustPressed(Keys.Right) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickRight) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.DPadRight))
                Selection++;

            if (Selection < 0)
                Selection = 2;
            if (Selection > 2)
                Selection = 0;

            // display different control scheme image based on selection
            switch (Selection)
            {
                case 0:
                    image = Globals.content.Load<Texture2D>("Menuscreens\\instruction1");
                    Globals.controlScheme = 0;
                    break;
                case 1:
                    image = Globals.content.Load<Texture2D>("Menuscreens\\instruction2");
                    Globals.controlScheme = 1;
                    break;
                case 2:
                    image = Globals.content.Load<Texture2D>("Menuscreens\\instruction3");
                    Globals.controlScheme = 2;
                    break;
            }

            if (Globals.input.KeyJustPressed(Keys.Enter) || Globals.input.KeyJustPressed(Keys.Escape) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.A) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.Start))
            {
                this.Destroy();
                Globals.screenManager.FindScreen("SettingsScreen").Activate();
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