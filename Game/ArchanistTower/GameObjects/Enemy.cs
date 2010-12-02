using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using ArchanistTower.Screens;

namespace ArchanistTower.GameObjects
{
    public class Enemy : GameObject
    {
        private Vector2 spriteWanderDirection;
        private float enemyWanderVelocity { get { return 0.75f; } }

        protected Texture2D lifebar;
        protected Stopwatch stopwatch;
        protected float enemyOrientation;
        protected float enemyTurnSpeed = 0.12f;
        protected EnemySpriteState enemyState = EnemySpriteState.Chase;
        public Vector2 PlayerPosition { get; set; }
        public List<Vector2> spellMotionList;
        public List<Vector2> spellPositionList;

        protected enum EnemySpriteState
        {
            Wander,
            Chase,
            Attack
        }

        public override void Initialize()
        {
            Collidable = true;
            stopwatch = new Stopwatch();
            base.Initialize();
        }


        /// <summary>
        /// Be sure to call base.update when inheriting
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            //don't allow sprite to leave gameworld
            SpriteAnimation.ClampToArea(
                   GameScreen.gameWorld.MapWidthInPixels,
                   GameScreen.gameWorld.MapHeightInPixels);            

            if (enemyState == EnemySpriteState.Wander)
            {   //if enemyState == Wander, make sprite wander, and adjust his speed
                Wander(SpriteAnimation.Position, ref spriteWanderDirection, ref enemyOrientation, enemyTurnSpeed);
                SpriteAnimation.Speed = enemyWanderVelocity;
            }

            Vector2 directUnitVect = new Vector2((float)Math.Cos(enemyOrientation), (float)Math.Sin(enemyOrientation));
            UpdateSpriteAnimation(directUnitVect);
            SpriteAnimation.Update(gameTime);
        }


        /// <summary>
        /// Logic for Collision between enemy and other GameObject
        /// </summary>
        /// <param name="obj">GameObject being collided with</param>
        public override void Collision(GameObject obj)
        {
            if (obj is Player)
            {
                if (!stopwatch.IsRunning)
                    stopwatch.Start();
            }
            else WorldCollision();
        }


        public virtual void Wander(Vector2 position, ref Vector2 wDirection, ref float orient, float turnSpeed)
        {
            wDirection.X += MathHelper.Lerp(-.25f, .25f, (float)Globals.random.NextDouble());
            wDirection.Y += MathHelper.Lerp(-.25f, .25f, (float)Globals.random.NextDouble());

            if (wDirection != Vector2.Zero)
            {
                wDirection.Normalize();
            }

            orient = TurnToFace(position, position + wDirection, orient, 15f * turnSpeed);

            Vector2 screenCenter = Vector2.Zero;
            screenCenter.X = Globals.graphics.GraphicsDevice.Viewport.Width / 2;
            screenCenter.Y = Globals.graphics.GraphicsDevice.Viewport.Height / 2;


            float distanceFromScreenCenter = Vector2.Distance(screenCenter, position);
            float MaxDistanceFromScreenCenter = Math.Min(screenCenter.Y, screenCenter.X);
            float normalizedDistance = distanceFromScreenCenter / MaxDistanceFromScreenCenter;
            float turnToCenterSpeed = .3f * normalizedDistance * normalizedDistance * turnSpeed;

            orient = TurnToFace(position, screenCenter, orient, turnToCenterSpeed);
        }

        public static float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed)
        {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;
            float desiredAngle = (float)Math.Atan2(y, x);
            float difference = MathHelper.WrapAngle(desiredAngle - currentAngle);

            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);
            return MathHelper.WrapAngle(currentAngle + difference);
        }

        /// <summary>
        /// Updates all information of the SpriteAnimation
        /// </summary>
        /// <param name="motion">Last motion vector</param>
        private void UpdateSpriteAnimation(Vector2 motion)
        {
            SpriteAnimation.IsAnimating = (SpriteAnimation.Speed != 0.0f) ? true : false; //if speed isn't 0, animating the sprite.
            LastMovement = SpriteAnimation.Speed * motion;   //LastMovement is a Vector2 of speed * motion
            SpriteAnimation.Position += LastMovement;        //position updated for LastMovement

            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);  //translate motion to radians
            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {   //if motion is pointed to right, adjust animation and facingdirection
                SpriteAnimation.CurrentAnimationName = "Right";
                Direction = FacingDirection.Right;
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {   //if motion is pointed down, etc.
                SpriteAnimation.CurrentAnimationName = "Down";
                Direction = FacingDirection.Down;
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {   //motion pointed up
                SpriteAnimation.CurrentAnimationName = "Up";
                Direction = FacingDirection.Up;
            }
            else
            {   //motion pointed left
                SpriteAnimation.CurrentAnimationName = "Left";
                Direction = FacingDirection.Left;
            }
        }
    }
}