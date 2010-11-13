using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledLib;


namespace ArchanistTower.GameObjects
{
    public class Spell : GameObject
    {
        public enum SpellType
        {
            Fire,
            Wind,
            Water
        }

        public SpellType selectedSpell;

        public FacingDirection direction;

        Vector2 motion = Vector2.Zero;

        public Spell(int i)
        {
            selectedSpell = (SpellType)i;

            SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Spells/spellsprites"));

            FrameAnimation fire = new FrameAnimation(4, 16, 16, 0, 0);
            fire.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Fire", fire);

            FrameAnimation wind = new FrameAnimation(4, 16, 16, 64, 0);
            wind.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Wind", wind);

            if ((int)selectedSpell == 0)
                SpriteAnimation.CurrentAnimationName = "Fire";
            if ((int)selectedSpell == 1)
                SpriteAnimation.CurrentAnimationName = "Wind";

            SpriteAnimation.IsAnimating = true;
        }

        public override void Initialize()
        { }

        public void LoadContent()
        { }

        public override void Update(GameTime gameTime)
        {
            SpriteAnimation.Position += motion * SpriteAnimation.Speed;
            SpriteAnimation.Update(gameTime);         
        }

        public override void Draw()
        {
            SpriteAnimation.Draw(Globals.spriteBatch);
        }

        public void Cast(string s, Vector2 p)
        {
            Vector2 offset;
            switch (s)
            {
                case "Up":
                    motion = new Vector2(0, -1);
                    offset = new Vector2(8, -16);
                    break;
                case "Down":
                    motion = new Vector2(0, 1);
                    offset = new Vector2(8, 24);
                    break;
                case "Left":
                    motion = new Vector2(-1, 0);
                    offset = new Vector2(0, 16);
                    break;
                case "Right":
                    motion = new Vector2(1, 0);
                    offset = new Vector2(24, 16);
                    break;
                default:
                    motion = Vector2.Zero;
                    offset = Vector2.Zero;
                    break;
            }
            SpriteAnimation.Position = p + offset;
        }
    }
}



