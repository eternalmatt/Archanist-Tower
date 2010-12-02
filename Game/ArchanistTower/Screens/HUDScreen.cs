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
        const int MAX_LIFEBARSIZE = 100;
        SpriteFont Font, ArialFont;
        AnimatedSprite crystal;
        Texture2D lifeBarTexture, manaBarTexture;
        Rectangle lifeBar = new Rectangle(0, 0, 0, 20);
        Rectangle manaBar = new Rectangle(0, 30, 0, 20);
        int PlayerHealth
        {
            get { return lifeBar.Width; }
            set { lifeBar.Width = value * 2; }
        }
        int PlayerMana
        {
            get { return manaBar.Width; }
            set { manaBar.Width = value * 2; }
        }


        public HUDScreen()
        {
            Initialize();
        }

        protected override void Unload() { }
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

            FrameAnimation none = new FrameAnimation(1, 0, 0, 0, 0);
            none.FramesPerSecond = 0;
            crystal.Animations.Add("None", none);

            crystal.CurrentAnimationName = "None";
            crystal.IsAnimating = true;
            crystal.Position = new Vector2(128, Globals.ScreenHeight - 32);

            lifeBarTexture = Globals.content.Load<Texture2D>("HUD/rectangle");
            manaBarTexture = Globals.content.Load<Texture2D>("HUD/rectangle_blue");
        }

        protected override void Update(GameTime gameTime)
        {
            PlayerHealth = GameWorld.Player.Health;
            PlayerMana = GameWorld.Player.Mana;
            crystal.Update(gameTime);
        }

        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            Globals.spriteBatch.Draw(lifeBarTexture, lifeBar, Color.White);
            Globals.spriteBatch.Draw(manaBarTexture, manaBar, Color.White);
            //Globals.spriteBatch.DrawString(Font, "Health:  " + PlayerHealth.ToString(), new Vector2(5, 5), Color.Red);
            //Globals.spriteBatch.DrawString(Font, "Mana:  " + PlayerMana.ToString(), new Vector2(5, 35), Color.Blue);
            Globals.spriteBatch.DrawString(ArialFont, "Current Spell:  ", new Vector2(5, Globals.ScreenHeight - 32), Color.AntiqueWhite);

            if (GameObjects.Player.selectedSpell == GameObjects.SelectedSpell.fire)
                crystal.CurrentAnimationName = "Fire";
            else if (GameObjects.Player.selectedSpell == GameObjects.SelectedSpell.wind)
                crystal.CurrentAnimationName = "Wind";
            else
            {
                //reserved for water spell
            }
            crystal.Draw(Globals.spriteBatch);

            Globals.spriteBatch.End(); 
        }
    }
}
