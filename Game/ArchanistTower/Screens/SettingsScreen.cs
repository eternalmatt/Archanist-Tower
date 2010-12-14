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
    class SettingsScreen : Screen
    {
        SpriteFont font;
        Texture2D image;
        Rectangle imageRectangle;
        string bgString, fxString;
        int Selection;

        public SettingsScreen()
        {
            
            Initialize();
        }

        protected override void Initialize()
        {
            image = Globals.content.Load<Texture2D>("Menuscreens\\settings");
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            font = Globals.content.Load<SpriteFont>("Fonts\\menufont");
            Selection = 0;
            Name = "SettingsScreen";
        }

        protected override void Unload()
        {  }

        protected override void Update(GameTime gameTime)
        {
#if WINDOWS
            if (Globals.input.KeyJustPressed(Keys.Up))
                Selection--;
            if (Globals.input.KeyJustPressed(Keys.Down))
                Selection++;

            if (Selection < 0)
                Selection = 2;
            if (Selection > 2)
                Selection = 0;

            if (Globals.input.KeyJustPressed(Keys.Enter))
                switch (Selection)
                {
                    case 0:
                        this.Disable();
                        Globals.screenManager.AddScreen(new InstructionScreen());
                        break;
                }

            if(Globals.input.KeyJustPressed(Keys.Left))
                switch (Selection)
                {
                    case 1:
                        Globals.fxVolume -= 10;
                        break;
                    case 2:
                        Globals.bgVolume -= 10;
                        break;
                }

            if (Globals.input.KeyJustPressed(Keys.Right))
                switch (Selection)
                {
                    case 1:
                        Globals.fxVolume += 10;
                        break;
                    case 2:
                        Globals.bgVolume += 10;
                        break;
                }

            if (Globals.fxVolume < 0)
                Globals.fxVolume = 0;
            if (Globals.fxVolume > 100)
                Globals.fxVolume = 100;
            if (Globals.bgVolume < 0)
                Globals.bgVolume = 0;
            if (Globals.bgVolume > 100)
                Globals.bgVolume = 100;

            bgString = "Background Music  < " + Globals.bgVolume + " >";
            fxString = "Effects Volume < " + Globals.fxVolume + " >";

            if (Globals.input.KeyJustPressed(Keys.Escape))
            {
                this.Destroy();
                if (Globals.screenManager.FindScreen("PauseScreen") != null)
                    Globals.screenManager.FindScreen("PauseScreen").Activate();
                else
                    Globals.screenManager.FindScreen("MenuScreen").Activate();
            }
#endif
            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickUp) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.DPadUp))
                Selection--;
            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickDown) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.DPadDown))
                Selection++;

            if (Selection < 0)
                Selection = 2;
            if (Selection > 2)
                Selection = 0;

            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.A))
                switch (Selection)
                {
                    case 0:
                        this.Disable();
                        Globals.screenManager.AddScreen(new InstructionScreen());
                        break;
                }

            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickLeft) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.DPadLeft))
                switch (Selection)
                {
                    case 1:
                        Globals.fxVolume -= 10;
                        break;
                    case 2:
                        Globals.bgVolume -= 10;
                        break;
                }

            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.LeftThumbstickRight) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.DPadRight))
                switch (Selection)
                {
                    case 1:
                        Globals.fxVolume += 10;
                        break;
                    case 2:
                        Globals.bgVolume += 10;
                        break;
                }

            if (Globals.fxVolume < 0)
                Globals.fxVolume = 0;
            if (Globals.fxVolume > 100)
                Globals.fxVolume = 100;
            if (Globals.bgVolume < 0)
                Globals.bgVolume = 0;
            if (Globals.bgVolume > 100)
                Globals.bgVolume = 100;

            bgString = "Background Music  < " + Globals.bgVolume + " >";
            fxString = "Effects Volume < " + Globals.fxVolume + " >";

            if (Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.Back) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.B))
            {
                this.Destroy();
                if (Globals.screenManager.FindScreen("PauseScreen") != null)
                    Globals.screenManager.FindScreen("PauseScreen").Activate();
                else
                    Globals.screenManager.FindScreen("MenuScreen").Activate();
            }
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(image, imageRectangle, Color.White);
            if (Selection == 0)
            {
                Globals.spriteBatch.DrawString(font, "CONTROLLER SETTINGS", new Vector2(300, 300), Color.Yellow);
                Globals.spriteBatch.DrawString(font, fxString, new Vector2(300, 375), Color.White);
                Globals.spriteBatch.DrawString(font, bgString, new Vector2(300, 450), Color.White);
            }
            else if (Selection == 1)
            {
                Globals.spriteBatch.DrawString(font, "CONTROLLER SETTINGS", new Vector2(300, 300), Color.White);
                Globals.spriteBatch.DrawString(font, fxString, new Vector2(300, 375), Color.Yellow);
                Globals.spriteBatch.DrawString(font, bgString, new Vector2(300, 450), Color.White);
            }
            else if (Selection == 2)
            {
                Globals.spriteBatch.DrawString(font, "CONTROLLER SETTINGS", new Vector2(300, 300), Color.White);
                Globals.spriteBatch.DrawString(font, fxString, new Vector2(300, 375), Color.White);
                Globals.spriteBatch.DrawString(font, bgString, new Vector2(300, 450), Color.Yellow);
            }

           // Globals.spriteBatch.DrawString(font, Globals.FXVolume().ToString(), new Vector2(200, 375), Color.Green);
            Globals.spriteBatch.End();
        }
    }
}
