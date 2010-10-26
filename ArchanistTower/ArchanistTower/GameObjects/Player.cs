using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using TileEngine;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower.GameObjects
{
    class Player : GameObject
    {
        AnimatedSprite sprite;

        public Player()
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();

            sprite = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/thf4"));

            /*
            FrameAnimation up = new FrameAnimation(2, 32, 32, 0, 0);
            up.FramesPerSecond = 10;
            sprite.Animations.Add("Up", up);

            FrameAnimation down = new FrameAnimation(2, 32, 32, 64, 0);
            down.FramesPerSecond = 10;
            sprite.Animations.Add("Down", down);

            FrameAnimation left = new FrameAnimation(2, 32, 32, 128, 0);
            left.FramesPerSecond = 10;
            sprite.Animations.Add("Left", left);

            FrameAnimation right = new FrameAnimation(2, 32, 32, 192, 0);
            right.FramesPerSecond = 10;
            sprite.Animations.Add("Right", right);            
       */ }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
