using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower.Collectables
{
    class Crystal : Collectable
    {
        enum CrystalType
        {
            blue,
            red,
            green
        }

        private CrystalType cType;

        public Crystal(string type, Vector2 position)
        {
            if(type.Equals("Blue"))
                cType = CrystalType.blue;
            if(type.Equals("Red"))
                cType = CrystalType.red;
            if(type.Equals("Green"))
                cType = CrystalType.green;

            Initialize(position);
        }

        public override void Collected()
        {
            if (cType == CrystalType.blue)
            {
                Globals.blue = 1;
            }
            if (cType == CrystalType.green)
            {
                Globals.green = 1;
            }
            if (cType == CrystalType.red)
            {
                Globals.red = 1;
            }
            Active = false;
        }

        public override void Initialize(Vector2 position)
        {
            Active = true;

            if(cType == CrystalType.blue)
            {
                SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Collectables/bluerelic"));
                FrameAnimation relic = new FrameAnimation(1, 32, 32, 0, 0);
                relic.FramesPerSecond = 1;
                SpriteAnimation.Animations.Add("Relic", relic);
            }
            if(cType == CrystalType.red)
            {
                SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Collectables/redrelic"));
                FrameAnimation relic = new FrameAnimation(1, 32, 32, 0, 0);
                relic.FramesPerSecond = 1;
                SpriteAnimation.Animations.Add("Relic", relic);
            }
            if(cType == CrystalType.green)
            {
                SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Collectables/greenrelic"));
                FrameAnimation relic = new FrameAnimation(1, 32, 32, 0, 0);
                relic.FramesPerSecond = 1;
                SpriteAnimation.Animations.Add("Relic", relic);
            }
            SpriteAnimation.CurrentAnimationName = "Relic";
            SpriteAnimation.Position = position;
        }

        public override void Update(GameTime gameTime)
        {
            if(Active)
                SpriteAnimation.Update(gameTime);
        }

        public override void Draw()
        {
            if (Active)
                SpriteAnimation.Draw(Globals.spriteBatch);
        }
    }
}
