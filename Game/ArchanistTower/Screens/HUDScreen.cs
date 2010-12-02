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
        #region Properties
        /*
         * lol, so the reason i put all these constants in here 
         * is so that our hud would be more modular, especially
         * when we needed to move everything for xbox's cutoff.
         * just change each number and the borders should hopefully scale.
         */
        const int offset_x = 10;    //x for lifebar to start
        const int offset_y = 10;    //y for lifebar to start
        const int maxWidth = 200;   //maxwidth of bars
        const int barHeight = 20;   //bar heigh
        const int manaStart = 10 + offset_y + barHeight;    //space between mana and lifebar

        const byte fadeValue = (byte)175;   //fade value of lifebars. number between 0 and 255.
        readonly Color FadedColor = new Color(Color.White, fadeValue); //our faded color
        
        List<Rectangle> BorderList;
        SpriteFont Font, ArialFont;
        AnimatedSprite crystal;
        Texture2D lifeBarTexture, manaBarTexture, borderTexture;
        Rectangle lifeBar = new Rectangle(offset_x, offset_y, maxWidth, barHeight);
        Rectangle manaBar = new Rectangle(offset_x, manaStart, maxWidth, barHeight);
        int PlayerHealth
        {
            get { return lifeBar.Width; }
            set { lifeBar.Width = value * maxWidth / 100; }
        }
        int PlayerMana
        {
            get { return manaBar.Width; }
            set { manaBar.Width = value * maxWidth / 100; }
        }
        #endregion

        #region Initialize
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
            //don't change ANY of these values;
            Rectangle borderAL = new Rectangle(offset_x, offset_y - 2, maxWidth, 2);         //above life
            Rectangle borderAM = new Rectangle(offset_x, manaStart - 2, maxWidth, 2);        //above mana
            Rectangle borderBL = new Rectangle(offset_x, offset_y + barHeight, maxWidth, 2); //below life
            Rectangle borderBM = new Rectangle(offset_x, manaStart + barHeight, maxWidth, 2);//below mana
            Rectangle borderRL = new Rectangle(offset_x + maxWidth, offset_y, 2, barHeight); //right of life
            Rectangle borderRM = new Rectangle(offset_x + maxWidth, manaStart, 2, barHeight);//right of mana
            Rectangle borderLL = new Rectangle(offset_x - 2, offset_y, 2, barHeight);        //left of life
            Rectangle borderLM = new Rectangle(offset_x - 2, manaStart, 2, barHeight);       //left of mana
            BorderList = new List<Rectangle>();
            BorderList.Add(borderBL);
            BorderList.Add(borderRL);
            BorderList.Add(borderAM);
            BorderList.Add(borderBM);
            BorderList.Add(borderRM);
            BorderList.Add(borderAL);
            BorderList.Add(borderLL);
            BorderList.Add(borderLM);
        }
        #endregion

        protected override void Update(GameTime gameTime)
        {
            PlayerHealth = GameWorld.Player.Health;
            PlayerMana = GameWorld.Player.Mana;
            crystal.Update(gameTime);
        }


        protected override void Draw()
        {
            Globals.spriteBatch.Begin();
            foreach (Rectangle border in BorderList)    //draw all the borders
                Globals.spriteBatch.Draw(borderTexture, border, Color.White);

            Globals.spriteBatch.DrawString(ArialFont, "Current Spell:  ", new Vector2(5, Globals.ScreenHeight - 32), Color.AntiqueWhite);
            if (GameObjects.Player.selectedSpell == GameObjects.SelectedSpell.fire)
                crystal.CurrentAnimationName = "Fire";
            else if (GameObjects.Player.selectedSpell == GameObjects.SelectedSpell.wind)
                crystal.CurrentAnimationName = "Wind";
            else
            { }     //reserved for water spell
            crystal.Draw(Globals.spriteBatch);
            Globals.spriteBatch.End();

            //whoever wrote SplashScreen.cs, thanks for insight on how to fade colors.
            Globals.spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            Globals.spriteBatch.Draw(lifeBarTexture, lifeBar, FadedColor);
            Globals.spriteBatch.Draw(manaBarTexture, manaBar, FadedColor);
            Globals.spriteBatch.End();
            //Globals.spriteBatch.DrawString(Font, GameWorld.Player.Health.ToString(), new Vector2(5, 10), Color.White);
            //Globals.spriteBatch.DrawString(Font, "Mana:  " + PlayerMana.ToString(), new Vector2(5, 35), Color.Blue);
        }
    }
}
