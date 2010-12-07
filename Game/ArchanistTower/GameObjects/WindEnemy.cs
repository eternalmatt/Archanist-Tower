using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.GameObjects
{
    class WindEnemy : ElementalEnemy
    {

        private const int enemyAttackRadius = 70;
        private const int enemyChaseRadius = 150;
        private float enemyAttackVelocity { get { return 1.2f; } }

        public WindEnemy(Vector2 startPosition)
        {
            Health = 100;
            Initialize();
            SpriteAnimation.Position = startPosition;
        }


        public override void Initialize()
        {
            SpriteAnimation = new AnimatedSprite(Globals.content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Sprites/Enemies/wmn3"));
            lifebar = Globals.content.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("HUD/rectangle");
            FrameAnimation up = new FrameAnimation(2, 32, 32, 0, 0);
            FrameAnimation down = new FrameAnimation(2, 32, 32, 64, 0);
            FrameAnimation left = new FrameAnimation(2, 32, 32, 128, 0);
            FrameAnimation right = new FrameAnimation(2, 32, 32, 192, 0);
            up.FramesPerSecond = down.FramesPerSecond = left.FramesPerSecond = right.FramesPerSecond = 10;
            SpriteAnimation.Animations.Add("Up", up);
            SpriteAnimation.Animations.Add("Down", down);
            SpriteAnimation.Animations.Add("Left", left);
            SpriteAnimation.Animations.Add("Right", right);
            SpriteAnimation.CurrentAnimationName = "Down";
            Direction = FacingDirection.Down;

            CollisionRadius = 64;
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
            {
                Dead = true;   //if Health < 0, dead = true and skip update logic
                Globals.EdeathFX.Play(Globals.FXVolume(), 0, 0);
            }
            else if (stopwatch.IsRunning)   //if stopwatch isrunning, sprite is paused, skip update logic
            {
                SpriteAnimation.IsAnimating = false;    //don't animate sprite
                if (stopwatch.Elapsed.Seconds >= 3) stopwatch.Reset(); //if sprite is paused for more than 3 second, reset stopwatch
            }
            else
            {
                CheckEnemyState();  //check the Wander/Chase/Attack state

                if (enemyState == EnemySpriteState.Attack)
                {   //if enemyState == Attack, make sprite attack player, and adjust his speed
                    Attack(SpriteAnimation.Position, PlayerPosition, ref enemyOrientation, enemyTurnSpeed);
                    SpriteAnimation.Speed = enemyAttackVelocity;
                }
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

        private void Attack(Vector2 position, Vector2 playerPosition, ref float orient, float turnSpeed)
        {   //change the oritenation to face player
            orient = TurnToFace(position, playerPosition, orient, turnSpeed);
        }
    }
}
