using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledLib;


namespace ArchanistTower
{
    public class Spell
    {
        AnimatedSprite sprite;

        public enum SpellType
        {
            Fire = 1,
            Wind,
            Water
        }

        public SpellType selectedSpell;

        public Spell()
        { }

        public void Initialize()
        {
            sprite = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Spells/spellsprites"));

            FrameAnimation fire = new FrameAnimation(4, 16, 16, 0, 0);
            fire.FramesPerSecond = 10;
            sprite.Animations.Add("Fire", fire);

            FrameAnimation wind = new FrameAnimation(4, 16, 16, 64, 0);
            wind.FramesPerSecond = 10;
            sprite.Animations.Add("Wind", wind);

            sprite.CurrentAnimationName = "Fire";
            selectedSpell = SpellType.Fire;

            sprite.IsAnimating = true;
        }

        public void LoadContent()
        {

        }

        public void Update(GameTime gameTime, Vector2 position)
        {
            if ((int)selectedSpell == 1)
                sprite.CurrentAnimationName = "Fire";
            if ((int)selectedSpell == 2)
                sprite.CurrentAnimationName = "Wind";
            sprite.Position = position;
            sprite.Update(gameTime);            
        }

        public void Draw()
        {
            Globals.spriteBatch.Begin(
                   SpriteBlendMode.AlphaBlend,
                   SpriteSortMode.Texture,
                   SaveStateMode.None,
                   Globals.camera.TransformMatrix);

            sprite.Draw(Globals.spriteBatch);

            Globals.spriteBatch.End();            
        }
    }
}



