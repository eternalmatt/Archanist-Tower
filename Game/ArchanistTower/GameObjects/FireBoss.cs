using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ArchanistTower.GameObjects
{
    class FireBoss : Enemy
    {

        private const int enemyAttackRadius = 140;
        private const int enemyChaseRadius = 300;

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

        /// <summary>
        /// Drawing JUST the lifebar above enemy's head.
        /// </summary>
        public override void Draw()
        {   //this requires a bit more logic than ElementalEnemies as Bosses have Health > 100
            int healthMod = Health;
            while (healthMod > 0)
            {
                if (healthMod % 100 != 0)
                {   //if Health == NNN where N isn't 0
                    Globals.spriteBatch.Draw(lifebar,
                        new Rectangle(SpriteAnimation.Bounds.X + 3, SpriteAnimation.Bounds.Y - 5 - 4 * (healthMod / 100 + 1),//x adjusted for spritebounds,y adjusted for spritebounds and each hundred of life
                            SpriteAnimation.Bounds.Width * (healthMod % 100) / 100 - 6, 2),//width based on sprite bounds and ammount health, height 2 pixels
                        Color.White);
                    healthMod -= healthMod % 100; //NNN = N00
                }
                else
                {   //if Health == N00 where N is a number
                    Globals.spriteBatch.Draw(lifebar,
                        new Rectangle(SpriteAnimation.Bounds.X + 3, SpriteAnimation.Bounds.Y - 5 - 4 * (healthMod / 100),//x adjusted for spritebounds,y adjusted for spritebounds and each hundred of life
                            SpriteAnimation.Bounds.Width - 6, 2),//width based on sprite bounds, height 2 pixels
                        Color.White);
                    healthMod -= 100;   //N00 -= 100
                }
            }
            base.Draw();
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0) Dead = true;   //if health <= 0, Dead = true and skip update logic
            else if (stopwatch.IsRunning)   //if stopwatch isrunning, boss is paused, skip update logic
            {
                SpriteAnimation.IsAnimating = false;    //don't animate sprite
                if (stopwatch.Elapsed.Seconds >= 1) stopwatch.Reset(); //if Boss is paused for more than 1 second, reset stopwatch
            }
            else
            {
                CheckEnemyState();

                if (enemyState == EnemySpriteState.Attack)
                {   //if enemyState == Attack, make Boss attack player, and adjust his speed
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
