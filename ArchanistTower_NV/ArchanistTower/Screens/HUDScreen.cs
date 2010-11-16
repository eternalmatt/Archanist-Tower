using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ArchanistTower.Screens
{
    class HUDScreen : Screen
    {
        SpriteFont Font, ArialFont;
        int PlayerHealth;

        AnimatedSprite crystal;

        public HUDScreen()
        {

            Initialize();
        }

        protected override void Unload()
        {
            throw new NotImplementedException();
        }
        
        protected override void Initialize()
        {
            Name = "HUDScreen";
            Font = Globals.content.Load<SpriteFont>("Fonts\\menufont");
            ArialFont = Globals.content.Load<SpriteFont>("Fonts\\Arial");

           // crystal = new AnimatedSprite(Globals.content.Load<Texture2D>("HUD/crystals"));
            crystal = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Spells/spellsprites"));

            FrameAnimation fire = new FrameAnimation(4, 16, 16, 0, 0);
            fire.FramesPerSecond = 10;
            crystal.Animations.Add("Fire", fire);

            FrameAnimation wind = new FrameAnimation(4, 16, 16, 64, 0);
            wind.FramesPerSecond = 10;
            crystal.Animations.Add("Wind", wind);

            crystal.CurrentAnimationName = "Fire";
            crystal.IsAnimating = true;
            crystal.Position = new Vector2(128, Globals.ScreenHeight - 32);
        }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            PlayerHealth = GameScreen.gameWorld.GameObjects[0].Health;

            crystal.Update(gameTime);
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.DrawString(Font, "Health:  " + PlayerHealth.ToString(), new Vector2(5, 5), Color.AntiqueWhite);
            Globals.spriteBatch.DrawString(ArialFont, "Current Spell:  ", new Vector2(5, Globals.ScreenHeight - 32), Color.AntiqueWhite);

            if (GameObjects.Player.selectedSpell == GameObjects.SelectedSpell.fire)
            {
                crystal.CurrentAnimationName = "Fire";
            }
            else if (GameObjects.Player.selectedSpell == GameObjects.SelectedSpell.wind)
            {
                crystal.CurrentAnimationName = "Wind";
            }
            else { }
            crystal.Draw(Globals.spriteBatch);

            Globals.spriteBatch.End(); 
        }
    }
}
