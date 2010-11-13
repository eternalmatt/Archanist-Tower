using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArchanistTower.Screens;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower.GameObjects
{
    class FireEnemy : Enemy
    {
        public FireEnemy(Vector2 startPosition)
        {
            Health = 100;
            Initialize();
            SpriteAnimation.Position = startPosition;
        }

        public override void Initialize()
        {
            SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Enemies/avt3"));

            FrameAnimation up = new FrameAnimation(2, 32, 32, 0, 0);
            up.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Up", up);

            FrameAnimation down = new FrameAnimation(2, 32, 32, 64, 0);
            down.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Down", down);

            FrameAnimation left = new FrameAnimation(2, 32, 32, 128, 0);
            left.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Left", left);

            FrameAnimation right = new FrameAnimation(2, 32, 32, 192, 0);
            right.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Right", right);

            SpriteAnimation.CurrentAnimationName = "Down";
            Direction = FacingDirection.Down;

            Collidable = true;
            CollisionRadius = 64;
        }

        public override void Update(GameTime gameTime)
        {
            SpriteAnimation.ClampToArea(
                   GameScreen.gameWorld.MapWidthInPixels,
                   GameScreen.gameWorld.MapHeightInPixels);
            SpriteAnimation.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
