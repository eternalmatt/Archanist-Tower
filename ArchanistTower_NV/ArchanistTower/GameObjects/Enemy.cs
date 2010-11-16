using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace ArchanistTower.GameObjects
{
    public class Enemy : GameObject
    {
        private Vector2 spriteWanderDirection;
        private const float enemyHysteresis = 15.0f;
        private float enemyWanderVelocity { get { return 0.75f; } }
        private float enemyChaseVelocity { get { return 0.57f; } }


        public float enemyOrientation;
        public float enemyTurnSpeed = 0.12f;
        public EnemySpriteState enemyState = EnemySpriteState.Chase;
        public enum EnemySpriteState
        {
            Wander,
            Chase,
            Attack
        }

        public override void Update(GameTime gameTime)
        {
            if (enemyState == EnemySpriteState.Wander)
            {
                Wander(SpriteAnimation.Position, ref spriteWanderDirection, ref enemyOrientation, enemyTurnSpeed);
                SpriteAnimation.Speed = enemyWanderVelocity;
            }
            else if (enemyState == EnemySpriteState.Chase)
            {
                Chase(SpriteAnimation.Position, PlayerPosition, ref enemyOrientation, enemyTurnSpeed);
                SpriteAnimation.Speed = enemyChaseVelocity;
            }


            Vector2 direction = new Vector2((float)Math.Cos(enemyOrientation), (float)Math.Sin(enemyOrientation));
            UpdateSpriteAnimation(direction);
            SpriteAnimation.IsAnimating = (SpriteAnimation.Speed != 0.0f) ? true : false;

            LastMovement = SpriteAnimation.Speed * direction;
            SpriteAnimation.Position += LastMovement;
        }
        

        public virtual void Chase(Vector2 position, Vector2 playerPosition, ref float orient, float turnSpeed)
        {
            orient = TurnToFace(position, playerPosition, orient, turnSpeed);
            /*
            if (position.X < playerPosition.X)
                position.X += enemyChaseVelocity.X;
            else position.X -= enemyChaseVelocity.X;

            if (position.Y < playerPosition.Y)
                position.Y += enemyChaseVelocity.Y;
            else position.Y -= enemyChaseVelocity.Y;
            */
            //return position;
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
            float difference = WrapAngle(desiredAngle - currentAngle);
            
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);
            return WrapAngle(currentAngle + difference);
        }

     
        private static float WrapAngle(float radians)
        {
            /*
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
            */
            return MathHelper.WrapAngle(radians);//not sure why its not this
        }

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 && motionAngle <= MathHelper.PiOver4)
            {
                SpriteAnimation.CurrentAnimationName = "Right";
                Direction = FacingDirection.Right;
            }
            else if (motionAngle >= MathHelper.PiOver4 && motionAngle <= 3f * MathHelper.PiOver4)
            {
                SpriteAnimation.CurrentAnimationName = "Down";
                Direction = FacingDirection.Down;
            }
            else if (motionAngle <= -MathHelper.PiOver4 && motionAngle >= -3f * MathHelper.PiOver4)
            {
                SpriteAnimation.CurrentAnimationName = "Up";
                Direction = FacingDirection.Up;
            }
            else
            {
                SpriteAnimation.CurrentAnimationName = "Left";
                Direction = FacingDirection.Down;
            }
        }
    }
}