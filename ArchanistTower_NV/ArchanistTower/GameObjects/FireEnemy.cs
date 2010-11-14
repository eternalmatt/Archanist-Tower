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

        private const int enemyAttackRadius = 70;
        private readonly Vector2 enemyAttackVelocity = new Vector2(1.2f, 1.2f);
        private const int enemyChaseRadius = 150;

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
            if (Health <= 0) Dead = true;
            else
            {
                SpriteAnimation.ClampToArea(
                       GameScreen.gameWorld.MapWidthInPixels,
                       GameScreen.gameWorld.MapHeightInPixels);
                SpriteAnimation.Update(gameTime);

                CheckEnemyState();


                if (enemyState == EnemySpriteState.Attack)
                
                    SpriteAnimation.Position = Attack(SpriteAnimation.Position, PlayerPosition, ref enemyOrientation, enemyTurnSpeed);
                

                base.Update(gameTime);
            }
        }

        private void CheckEnemyState()
        {
            int distance = (int)Vector2.Distance(SpriteAnimation.Position, PlayerPosition);
            if (distance < enemyAttackRadius)
                enemyState = EnemySpriteState.Attack;
            else if (distance < enemyChaseRadius)
                enemyState = EnemySpriteState.Chase;
            else
                enemyState = EnemySpriteState.Wander;
        }

        private Vector2 Attack(Vector2 position, Vector2 playerPosition, ref float orient, float turnSpeed) 
        {
            orient = TurnToFace(position, playerPosition, orient, turnSpeed);

            if (position.X < playerPosition.X)
                position.X += enemyAttackVelocity.X;
            else position.X -= enemyAttackVelocity.X;

            if (position.Y < playerPosition.Y)
                position.Y += enemyAttackVelocity.Y;
            else position.Y -= enemyAttackVelocity.Y;

            return position;
        }
    }
}
