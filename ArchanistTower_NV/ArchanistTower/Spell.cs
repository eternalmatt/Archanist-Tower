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

        const float velocity = 1.5f;

        enum SpellType
        {
            Fire,
            Water,
            Wind
        }

        public Spell()
        {

        }

        public void Initialize()
        {
            sprite = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Spells/fireball"));

            FrameAnimation animateFireball = new FrameAnimation(4, 16, 16, 0, 0);
            animateFireball.FramesPerSecond = 10;
            sprite.Animations.Add("AnimateFireball", animateFireball);

            sprite.CurrentAnimationName = "AnimateFireball";
            sprite.IsAnimating = true;
        }

        public void LoadContent()
        {

        }

        public void Update(GameTime gameTime, Vector2 position)
        {
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



