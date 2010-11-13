using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArchanistTower.GameObjects
{
    public abstract class Enemy : GameObject
    {
        private float enemyOrientation;
        private Vector2 spriteWanderDirection;
        private Random random = new Random();
        private float enemySpeed = 1.5f;
        private float enemyTurnSpeed = 0.12f;
        private float enemyChaseDistance = 270.0f;
        private float enemyAttackDistance = 30.0f;
        private const float enemyHysteresis = 15.0f;

        public FacingDirection Direction { get; set; }

        EnemySpriteState enemyState = EnemySpriteState.Wander;
        enum EnemySpriteState
        {
            Wander,
            Chase,
            Attack
        }

        public override void Update(GameTime gameTime)
        {
            float currentSpeed = 0.0f;
            if (enemyState == EnemySpriteState.Wander)
            {
                //Wander(SpriteAnimation.Position, ref spriteWanderDirection, ref enemyOrientation, enemyTurnSpeed);
                //currentSpeed = .25f * enemySpeed;
            }

            Vector2 direction = new Vector2((float)Math.Cos(enemyOrientation), (float)Math.Sin(enemyOrientation));
            SpriteAnimation.Position += direction * currentSpeed;


            UpdateSpriteAnimation(direction);
            SpriteAnimation.IsAnimating = (currentSpeed != 0.0f) ? true : false;
        }

        private void Wander(Vector2 position, ref Vector2 wDirection, ref float orient, float turnSpeed)
        {
            wDirection.X += MathHelper.Lerp(-.25f, .25f, (float)random.NextDouble());
            wDirection.Y += MathHelper.Lerp(-.25f, .25f, (float)random.NextDouble());

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

        private static float TurnToFace(Vector2 position, Vector2 faceThis, float currentAngle, float turnSpeed)
        {
            float x = faceThis.X - position.X;
            float y = faceThis.Y - position.Y;
            float desiredAngle = (float)Math.Atan2(y, x);
            float difference = WrapAngle(desiredAngle - currentAngle);
            
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);
            return WrapAngle(currentAngle + difference);
        }

     
        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
            //return MathHelper.WrapAngle(radians);//not sure why its not this
        }

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
                SpriteAnimation.CurrentAnimationName = "Right";
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
                SpriteAnimation.CurrentAnimationName = "Down";
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
                SpriteAnimation.CurrentAnimationName = "Up";
            else
                SpriteAnimation.CurrentAnimationName = "Left";
        }
    }
}