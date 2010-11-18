using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArchanistTower.Screens;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ArchanistTower.GameObjects
{
    class FireBoss : Enemy
    {

        private const int enemyAttackRadius = 140;
        private const int enemyChaseRadius = 300;
        private Stopwatch stopwatch;
        private Texture2D lifebar;

        private float enemyAttackVelocity { get { return 1.5f; } }

        public FireBoss(Vector2 startPosition)
        {
            Health = 300;
            Initialize();
            SpriteAnimation.Position = startPosition;
        }


        public override void Initialize()
        {
            SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Enemies/amg1"));
            lifebar = Globals.content.Load<Texture2D>("HUD/rectangle");

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
            stopwatch = new Stopwatch(); 
            base.Initialize();
        }

        public override void Draw()
        {
            int healthMod = Health;
            while (healthMod > 0)
            {
                //sorry for this mess of code. it works but its not very clean
                //basically what i'm doing is looping through each hundred of Health
                //and then drawing that line to the screen
                //I might replace with a proper for() loop later
                Globals.spriteBatch.Draw(
                    lifebar,
                    new Rectangle(SpriteAnimation.Bounds.X + 3, SpriteAnimation.Bounds.Y - 5 - 4 * (healthMod % 100 != 0 ? healthMod / 100 + 1 : healthMod / 100),   //x and y adjusted for sprite and health
                        SpriteAnimation.Bounds.Width * (healthMod % 100 != 0 ? healthMod % 100 : 100) / 100 - 6, 2),    //width based on health / adjusted for sprite
                    Color.White
                );
                if (healthMod % 100 == 0) healthMod -= 100;
                else healthMod -= healthMod % 100;
            }
            base.Draw();
        }

        public override void Update(GameTime gameTime)
        {
            if (stopwatch.IsRunning && stopwatch.Elapsed.Seconds >= 1)
                stopwatch.Reset();

            if (Health <= 0) Dead = true;
            else if (stopwatch.IsRunning) SpriteAnimation.IsAnimating = false;
            else
            {
                SpriteAnimation.ClampToArea(
                       GameScreen.gameWorld.MapWidthInPixels,
                       GameScreen.gameWorld.MapHeightInPixels);
                SpriteAnimation.Update(gameTime);

                CheckEnemyState();


                if (enemyState == EnemySpriteState.Attack)
                {
                    Attack(SpriteAnimation.Position, PlayerPosition, ref enemyOrientation, enemyTurnSpeed);
                    SpriteAnimation.Speed = enemyAttackVelocity;
                }

                base.Update(gameTime);
            }
        }




        public override void Collision(GameObject obj)
        {
            if (obj is Player)
            {
                if (!stopwatch.IsRunning)
                    stopwatch.Start();
            }
            else if (obj.GetType() == typeof(FireEnemy))
            {
                WorldCollision();
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

        private void Attack(Vector2 position, Vector2 playerPosition, ref float orient, float turnSpeed) 
        {
            orient = TurnToFace(position, playerPosition, orient, turnSpeed);
            /*
            if (position.X < playerPosition.X)
                position.X += enemyAttackVelocity.X;
            else position.X -= enemyAttackVelocity.X;

            if (position.Y < playerPosition.Y)
                position.Y += enemyAttackVelocity.Y;
            else position.Y -= enemyAttackVelocity.Y;
             */

            //return position;
        }
    }
}
