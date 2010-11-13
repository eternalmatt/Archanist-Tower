using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ArchanistTower.Screens;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;

namespace ArchanistTower.GameObjects
{
    public enum SelectedSpell
    {
        fire,
        wind,
        water
    }

    public class Player : GameObject
    {
        protected Keys MoveLeft;
        protected Keys MoveRight;
        protected Keys MoveUp;
        protected Keys MoveDown;
        protected Keys SpellCast;

        protected bool red;
        protected bool blue;
        protected bool green;
        private FacingDirection direction;
        SelectedSpell selectedSpell;

        public Player(Vector2 startPosition)
        {
            Initialize();
            SpriteAnimation.Position = startPosition;
        }

        public override void Initialize()
        {
            SpriteAnimation = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Player/man1"));

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
            direction = FacingDirection.Down;

            SetKeys(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Space);

            Collidable = true;
            CollisionRadius = 64;
        }

        public override void Update(GameTime gameTime)
        {
            InputCheck();

            SpriteAnimation.ClampToArea(
                GameScreen.gameWorld.MapWidthInPixels,
                GameScreen.gameWorld.MapHeightInPixels);
            SpriteAnimation.Update(gameTime);

            Globals.camera.LockToTarget(SpriteAnimation, Globals.ScreenWidth, Globals.ScreenHeight);

            Globals.camera.ClampToArea(
                GameScreen.gameWorld.MapWidthInPixels - Globals.ScreenWidth,
                GameScreen.gameWorld.MapHeightInPixels - Globals.ScreenHeight);

        }

        private void InputCheck()
        {
            Vector2 movement = Vector2.Zero;

            if (Globals.input.KeyPressed(MoveRight))
            {
                movement.X = 1;
                direction = FacingDirection.Right;
            }
            else if (Globals.input.KeyPressed(MoveLeft))
            {
                movement.X = -1;
                direction = FacingDirection.Left;
            }
            if (Globals.input.KeyPressed(MoveUp))
            {
                movement.Y = -1;
                direction = FacingDirection.Up;
            }
            else if (Globals.input.KeyPressed(MoveDown))
            {
                movement.Y = 1;
                direction = FacingDirection.Down;
            }

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                UpdateSpriteAnimation(movement);
                SpriteAnimation.IsAnimating = true;
            }
            else
                SpriteAnimation.IsAnimating = false;

            SpriteAnimation.Position += SpriteAnimation.Speed * movement;
            LastMovement = SpriteAnimation.Speed * movement;

            if (Globals.input.KeyPressed(Keys.F1))
                selectedSpell = SelectedSpell.fire;
            if (Globals.input.KeyPressed(Keys.F2))
                selectedSpell = SelectedSpell.wind;

            if (Globals.input.KeyJustPressed(SpellCast))
            {
                if(selectedSpell == SelectedSpell.fire)
                    GameScreen.gameWorld.AddObject(new FireSpell(direction, SpriteAnimation.Position));
            }
        }

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 &&
                motionAngle <= MathHelper.PiOver4)
            {
                SpriteAnimation.CurrentAnimationName = "Right";
                //motion = new Vector2(1f, 0f);  //These lines restrict diagonal movement
            }
            else if (motionAngle >= MathHelper.PiOver4 &&
                motionAngle <= 3f * MathHelper.PiOver4)
            {
                SpriteAnimation.CurrentAnimationName = "Down";
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 &&
                motionAngle >= -3f * MathHelper.PiOver4)
            {
                SpriteAnimation.CurrentAnimationName = "Up";
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                SpriteAnimation.CurrentAnimationName = "Left";
                //motion = new Vector2(-1f, 0f);
            }
            //sprite.Position += motion * sprite.Speed; //must be after for tile based movement
        }

        private void CastSpell()
        {
            //GameScreen.gameWorld
        }

        public void SetKeys(Keys left, Keys right, Keys up, Keys down, Keys cast)
        {
            MoveLeft = left;
            MoveRight = right;
            MoveUp = up;
            MoveDown = down;
            SpellCast = cast;
        }


        public override void Collision(GameObject o)
        {
            if (o is Enemy)
            {
                if (o is FireEnemy)
                {
                    EnemyCollision();
                    Health -= 15;
                }
            }
            //Not sure that spell will be handled here...maybe in the spell class.
            if (o.GetType() == typeof(Spell))
            {
                if (o.GetType() == typeof(FireSpell))
                {

                }
            }
        }


        private void EnemyCollision()
        {
            switch (direction)
            {
                case FacingDirection.Left:
                    SpriteAnimation.Position.X += 20;
                    break;

            }
        }
    }

}
