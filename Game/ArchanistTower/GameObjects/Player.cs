using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ArchanistTower.Screens;
using Microsoft.Xna.Framework.Graphics;
using TiledLib;
using System.Diagnostics;

namespace ArchanistTower.GameObjects
{
    public enum SelectedSpell
    {
        none,
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
        protected Keys CastLeft;
        protected Keys CastRight;
        protected Keys CastUp;
        protected Keys CastDown;

        protected bool red;
        protected bool blue;
        protected bool green;
        public static SelectedSpell selectedSpell;
        private Stopwatch stopwatch;

        public Player(Vector2 startPosition)
        {
            Health = 100;
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
            Direction = FacingDirection.Down;
            selectedSpell = SelectedSpell.none;

            SetKeys(Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space, Keys.Left, Keys.Right, Keys.Up, Keys.Down);

            Collidable = true;
            CollisionRadius = 64;
            stopwatch = new Stopwatch();
        }

        public override void Update(GameTime gameTime)
        {
            if (stopwatch.ElapsedMilliseconds > 1500) stopwatch.Reset();
            if (stopwatch.IsRunning) Collidable = false;
            else Collidable = true;

            InputCheck();

            SpriteAnimation.ClampToArea(
                GameScreen.gameWorld.MapWidthInPixels,
                GameScreen.gameWorld.MapHeightInPixels);
            SpriteAnimation.Update(gameTime);

            Globals.camera.LockToTarget(SpriteAnimation, Globals.ScreenWidth, Globals.ScreenHeight);

            Globals.camera.ClampToArea(
                GameScreen.gameWorld.MapWidthInPixels - Globals.ScreenWidth,
                GameScreen.gameWorld.MapHeightInPixels - Globals.ScreenHeight);

            if (Globals.blue == 1)
                blue = true;
            if (Globals.red == 1)
                red = true;
            if (Globals.green == 1)
                green = true;
            /*int i = ShaderCode.effectPost.Parameters["powerGained"].GetValueInt32();
            if (i > 0)
            {
                if (i >= 1)
                {
                    red = true;
                    if (i >= 2)
                    {
                        green = true;
                        if (i >= 3)
                            blue = true;
                    }
                }
            }*/
        }


        private void InputCheck()
        {
            Vector2 movement = Vector2.Zero;

            if (Globals.input.KeyPressed(MoveRight))
            {
                movement.X = 1;
                Direction = FacingDirection.Right;
            }
            else if (Globals.input.KeyPressed(MoveLeft))
            {
                movement.X = -1;
                Direction = FacingDirection.Left;
            }
            if (Globals.input.KeyPressed(MoveUp))
            {
                movement.Y = -1;
                Direction = FacingDirection.Up;
            }
            else if (Globals.input.KeyPressed(MoveDown))
            {
                movement.Y = 1;
                Direction = FacingDirection.Down;
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

            if (red || green || blue)
            {
                if (Globals.input.KeyJustPressed(SpellCast))
                {
                    if (Globals.input.KeyPressed(CastLeft))
                        Direction = FacingDirection.Left;
                    else if (Globals.input.KeyPressed(CastRight))
                        Direction = FacingDirection.Right;
                    else if (Globals.input.KeyPressed(CastUp))
                        Direction = FacingDirection.Up;
                    else if (Globals.input.KeyPressed(CastDown))
                        Direction = FacingDirection.Down;

                    if (selectedSpell == SelectedSpell.fire && red)
                        GameWorld.Spells.Add(new FireSpell(Direction, SpriteAnimation.Position) { originatingType = "Player" });
                    else if (selectedSpell == SelectedSpell.wind && green)
                        GameWorld.Spells.Add(new WindSpell(Direction, SpriteAnimation.Position) { originatingType = "Player" });
                    else if (selectedSpell == SelectedSpell.water && blue)
                    { } // reserved for water spell
                }
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

        public void SetKeys(Keys left, Keys right, Keys up, Keys down, Keys cast, Keys cLeft, Keys cRight, Keys cUp, Keys cDown)
        {
            MoveLeft = left;
            MoveRight = right;
            MoveUp = up;
            MoveDown = down;
            SpellCast = cast;
            CastLeft = cLeft;
            CastRight = cRight;
            CastUp = cUp;
            CastDown = cDown;
        }


        public override void Collision(GameObject o)
        {
            if (o is Enemy)
            {
                if (!stopwatch.IsRunning) stopwatch.Start();
                o.Collision(this);

                if (o is FireEnemy)
                {
                    EnemyCollision(o.Direction);
                    Health -= 5;
                }
                else if (o is FireBoss)
                {
                    EnemyCollision(o.Direction);
                    Health -= 10;
                }
            }
            //Not sure that spell will be handled here...maybe in the spell class.
            else if (o.GetType() == typeof(Spell))
            {
                if (o.GetType() == typeof(FireSpell))
                {

                }
            }

            if (Health <= 0)
            {
                GameScreen.GameOver();
            }
        }


        private void EnemyCollision(FacingDirection Direction)
        {
            switch (Direction)
            {
                case FacingDirection.Left:
                    SpriteAnimation.Position.X -= 20;
                    break;
                case FacingDirection.Right:
                    SpriteAnimation.Position.X += 20;
                    break;
                case FacingDirection.Up:
                    SpriteAnimation.Position.Y -= 20;
                    break;
                case FacingDirection.Down:
                    SpriteAnimation.Position.Y += 20;
                    break;
            }
        }

        public override void Draw()
        {
            if (!   (stopwatch.IsRunning && (stopwatch.Elapsed.Milliseconds / 100) % 2 == 0))
                base.Draw();
        }
    }

}
