using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using TileEngine;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower.GameObjects
{
    public class Enemy : GameObject
    {
        AnimatedSprite sprite;

        Vector2 spritePos;
        EnemySpriteState enemyState = EnemySpriteState.Wander;
        float enemyOrientation;
        Vector2 spriteWanderDirection;
        Random random = new Random();

        const float enemySpeed = 1.5f;
        const float enemyTurnSpeed = 0.12f;
        const float enemyChaseDistance = 270.0f;
        const float enemyAttackDistance = 30.0f;
        const float enemyHysteresis = 15.0f;




        enum EnemySpriteState
        {
            Wander,
            Chase,
            Attack
        }

        public Enemy()
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();

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

            sprite.CurrentAnimationName = "Down";
            sprite.Position = new Vector2(100, 400);
         }

        public void LoadContent()
        {
            sprite = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/amg1"));
            sprite.OriginOffset = new Vector2(16, 32);
        }

        public override void Update(GameTime gameTime)
        {
            
            float enemyChase = enemyChaseDistance;
            float enemyAttack = enemyAttackDistance;

            if (enemyState == EnemySpriteState.Wander)
            {
                enemyChase -= enemyHysteresis / 2;
            }
            else if (enemyState == EnemySpriteState.Chase)
            {
                enemyChase += enemyHysteresis / 2;
                enemyAttack -= enemyHysteresis / 2;
            }
            else if (enemyState == EnemySpriteState.Attack)
            {
                enemyAttack += enemyHysteresis / 2;
            }

            float distanceFromPlayer = Vector2.Distance(sprite.Position, Player.playerSprite.Position);
            if (distanceFromPlayer > enemyChase)
            {
                enemyState = EnemySpriteState.Wander;
            }
            else if (distanceFromPlayer > enemyAttack)
            {
                enemyState = EnemySpriteState.Chase;
            }
            else
            {
                enemyState = EnemySpriteState.Attack;
            }

            float currentSpeed;
            if (enemyState == EnemySpriteState.Chase)
            {
                enemyOrientation = TurnToFace(sprite.Position, Player.playerSprite.Position, enemyOrientation, enemyTurnSpeed);
                currentSpeed = enemySpeed;
            }
            else if (enemyState == EnemySpriteState.Wander)
            {
                Wander(sprite.Position, ref spriteWanderDirection, ref enemyOrientation, enemyTurnSpeed);
                currentSpeed = .25f * enemySpeed;
            }
            else
            {
                currentSpeed = 0.0f;
            }
        
       

            Vector2 direction = new Vector2((float)Math.Cos(enemyOrientation), (float)Math.Sin(enemyOrientation));
            sprite.Position += direction * currentSpeed;
           

            UpdateSpriteAnimation(direction);
            sprite.IsAnimating = true;

            if (currentSpeed == 0.0f)
            {
                sprite.IsAnimating = false;
            }

            sprite.Update(gameTime);

            base.Update(gameTime);
        }


        private void Wander(Vector2 position, ref Vector2 wDirection,
           ref float orient, float turnSpeed)
        {
            
       
            wDirection.X +=
                MathHelper.Lerp(-.25f, .25f, (float)random.NextDouble());
            wDirection.Y +=
                MathHelper.Lerp(-.25f, .25f, (float)random.NextDouble());

            if (wDirection != Vector2.Zero)
            {
                wDirection.Normalize();
            }
          
            orient = TurnToFace(position, position + wDirection, orient,
                .15f * turnSpeed);


            Vector2 screenCenter = Vector2.Zero;
            screenCenter.X = Globals.graphics.GraphicsDevice.Viewport.Width / 2;
            screenCenter.Y = Globals.graphics.GraphicsDevice.Viewport.Height / 2;

            
            float distanceFromScreenCenter = Vector2.Distance(screenCenter, position);
            float MaxDistanceFromScreenCenter =
                Math.Min(screenCenter.Y, screenCenter.X);

            float normalizedDistance =
                distanceFromScreenCenter / MaxDistanceFromScreenCenter;

            float turnToCenterSpeed = .3f * normalizedDistance * normalizedDistance *
                turnSpeed;

            
            orient = TurnToFace(position, screenCenter, orient,
                turnToCenterSpeed);
        }


        private static float TurnToFace(Vector2 position, Vector2 faceThis,
            float currentAngle, float turnSpeed)
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
        }

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 &&
                motionAngle <= MathHelper.PiOver4)
            {
                sprite.CurrentAnimationName = "Right";
                //motion = new Vector2(1f, 0f);  //These lines restrict diagonal movement
            }
            else if (motionAngle >= MathHelper.PiOver4 &&
                motionAngle <= 3f * MathHelper.PiOver4)
            {
                sprite.CurrentAnimationName = "Down";
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 &&
                motionAngle >= -3f * MathHelper.PiOver4)
            {
                sprite.CurrentAnimationName = "Up";
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                sprite.CurrentAnimationName = "Left";
                //motion = new Vector2(-1f, 0f);
            }
            //sprite.Position += motion * sprite.Speed; //must be after for tile based movement
        }

      

        public override void Draw()
        {
            sprite.Draw(Globals.spriteBatch);

            base.Draw();
        }
    }
}
        

 
