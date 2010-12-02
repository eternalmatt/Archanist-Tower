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
        const int max = 200;
        const int manaStart = 30;
        const int barHeight = 20;
        SpriteFont Font, ArialFont;
        AnimatedSprite crystal;
        Texture2D lifeBarTexture, manaBarTexture, borderTexture;
        Rectangle lifeBar = new Rectangle(0, 0, 0, barHeight);
        Rectangle manaBar = new Rectangle(0, manaStart, 0, barHeight);
        Rectangle borderA = new Rectangle(0, barHeight, max, 2);
        Rectangle borderB = new Rectangle(max, 0, 2, barHeight);
        Rectangle borderC = new Rectangle(0, manaStart - 2, max, 2);
        Rectangle borderD = new Rectangle(0, manaStart + barHeight, max, 2);
        Rectangle borderE = new Rectangle(max, manaStart, 2, barHeight);
        List<Rectangle> BorderList;
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

            borderTexture = Globals.content.Load<Texture2D>("HUD/border");
            lifeBarTexture = Globals.content.Load<Texture2D>("HUD/rectangle");
            manaBarTexture = Globals.content.Load<Texture2D>("HUD/rectangle_blue");
            BorderList = new List<Rectangle>();
            BorderList.Add(borderA);
            BorderList.Add(borderB);
            BorderList.Add(borderC);
            BorderList.Add(borderD);
            BorderList.Add(borderE);
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
            foreach (Rectangle border in BorderList)
                Globals.spriteBatch.Draw(borderTexture, border, Color.White);
            Globals.spriteBatch.Draw(borderTexture, borderA, Color.White);
            Globals.spriteBatch.Draw(borderTexture, borderB, Color.White);
            
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
