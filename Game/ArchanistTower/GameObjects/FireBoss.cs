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
        private const int runFromSpellRadius = 100;
        private const float runFromSpellSpeed = 1.5f;
        private float enemyAttackVelocity { get { return 1.5f; } }
        private Stopwatch spellwatch;

        public FireBoss(Vector2 startPosition)
        {
            Health = 300;
            Initialize();
            SpriteAnimation.Position = startPosition;
            spellwatch = new Stopwatch();
        }


        private Vector2 AvoidClosestSpell()
        {
            if (spellPositionList.Count > 0)
            {
                int index = 0;
                int closest = (int)Vector2.DistanceSquared(SpriteAnimation.Position, spellPositionList[0]);
                for (int i = 1; i < spellPositionList.Count; i++)
                    if (Vector2.DistanceSquared(SpriteAnimation.Position, spellPositionList[i]) < closest)
                    {
                        closest = (int)Vector2.DistanceSquared(SpriteAnimation.Position, spellPositionList[i]);
                        index = i;
                    }

                Vector2 cPosition = spellPositionList[index];   //for closest spell position
                Vector2 cMotion = spellMotionList[index];       //for closest spell motion

                //http://www.intmath.com/Plane-analytic-geometry/Perpendicular-distance-point-line.php this link helped me with some math
                int A = (int)(cMotion.Y / cMotion.X) * -1;
                int C = (int)(-1 * A * cPosition.X - cPosition.Y);
                int distance = (int)Math.Abs((-1* A * cPosition.X + cPosition.Y + C) / A);

                if (distance < runFromSpellRadius)
                {
                    //this is the vector perpendicular to the closest spell's motion vector (i hope)
                    Vector2 perpendicular = new Vector2(cMotion.Y * -1, cMotion.X);
                    perpendicular.Normalize();
                    return perpendicular;
                }
                else return Vector2.Zero;
            }
            else return Vector2.Zero;
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
        /// Drawing JUST the lifebar above BOSS's head.
        /// </summary>
        public override void Draw()
        {   //this requires a bit more logic than ElementalEnemies as Bosses have Health > 100
            int healthTemp;  //a temp variable
            Rectangle rectangle = new Rectangle(SpriteAnimation.Bounds.X + 3, //X based on SpriteAnimation
                SpriteAnimation.Bounds.Y - 5 - 4 * (Health / 100),  //Y Based on SpriteAnimation and health
                SpriteAnimation.Bounds.Width - 6,   //width based on sprite's width
                2);     //height is 2 pixels

            if (Health % 100 != 0)
            {   //if Health == NNN where N isn't 0
                rectangle.Width = SpriteAnimation.Bounds.Width * (Health % 100) / 100 - 6; //width adjusted to health
                Globals.spriteBatch.Draw(lifebar, rectangle, Color.White);  //draw first lifebar
                healthTemp = Health - Health % 100; //NNN = N00
                rectangle.Width = SpriteAnimation.Bounds.Width - 6; //correct width for rest of life bars 
            }
            else
            {   //if Health == N00 where N is a number
                Globals.spriteBatch.Draw(lifebar, rectangle, Color.White); //draw first lifebar
                healthTemp = Health - 100;   //N00 -= 100
            }

            for (int i = healthTemp / 100; i > 0; i--)  //loop through each 100 of health
            {
                rectangle.Y += 4;   //increment Y position to draw
                Globals.spriteBatch.Draw(lifebar, rectangle, Color.White); //draw health's rectangles
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

                if (spellPositionList.Count > 0)
                    RunFromPlayerSpell(SpriteAnimation.Position, AvoidClosestSpell(), ref enemyOrientation, enemyTurnSpeed);

                if (enemyState == EnemySpriteState.Attack)
                    //if enemyState == Attack, make Boss attack player, and adjust his speed
                    Attack(SpriteAnimation.Position, PlayerPosition, ref enemyOrientation, enemyTurnSpeed);
                

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
            if (spellwatch.ElapsedMilliseconds > 1000) spellwatch.Reset();

            if (!spellwatch.IsRunning)
            {
                spellwatch.Start();
                GameWorld.Spells.Add(new FireSpell(playerPosition, position) { originatingType = GameObjects.Spell.OriginatingType.Enemy });
                //SpriteAnimation.Speed = enemyAttackVelocity;
            }
        }

        private void RunFromPlayerSpell(Vector2 position, Vector2 perpendicular, ref float orient, float turnspeed)
        {
            orient = TurnToFace(position, perpendicular, orient, turnspeed);
            SpriteAnimation.Speed = runFromSpellSpeed;
        }
    }
}
