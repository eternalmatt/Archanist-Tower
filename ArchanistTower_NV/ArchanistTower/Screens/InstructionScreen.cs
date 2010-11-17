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

        public InstructionScreen()
        {
            image = Globals.content.Load<Texture2D>("Menuscreens\\instruction");
            imageRectangle = new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight);
            Initialize();
        }

        protected override void Initialize()
        {
            Name = "InstructionScreen";
        }

        protected override void Unload()
        { }

        protected override void Update(GameTime gameTime)
        {    
            if (Globals.input.KeyJustPressed(Keys.Enter) ||
                Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.A))
            {
                this.Destroy();
                if (Globals.screenManager.FindScreen("GameScreen") == null) // Game not started yet
                {
                    Globals.screenManager.AddScreen(new GameScreen());
                }
                else // Instruction screen is called from pause screen
                {
                    Globals.screenManager.FindScreen("PauseScreen").Activate();
                }
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