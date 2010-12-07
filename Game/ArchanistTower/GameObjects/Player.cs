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
using Microsoft.Xna.Framework.Audio;

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
        const int MANA_RECHARGE = 5;
        const int FIRE_SPELL_MANA = 12;
        const int WIND_SPELL_MANA = 9;

        public int Mana;
        float timer;

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
        public bool HasBeenHit { get { return stopwatch.IsRunning; } }
        private int currentControlScheme;

        public Player(Vector2 startPosition)
        {
            Health = 100;
            Mana = 100;
            timer = 0f;
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

            currentControlScheme = Globals.controlScheme;
            CheckControlScheme();

            Collidable = true;
            CollisionRadius = 64;
            stopwatch = new Stopwatch();


        }

        #region Update

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0 && Globals.I_AM_INVINCIBLE == false)
            {
                Dead = true;
                Globals.PdeathFX.Play(Globals.FXVolume(), 0, 0);
            }
            
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
                GameScreen.gameWorld.MapWidthInPixels - Globals.ScreenWidth, Globals.ScreenWidth,
                GameScreen.gameWorld.MapHeightInPixels - Globals.ScreenHeight, Globals.ScreenHeight);

            if (Globals.blue == 1)
                blue = true;
            if (Globals.red == 1)
                red = true;
            if (Globals.green == 1)
                green = true;

            timer += (float)gameTime.ElapsedGameTime.TotalSeconds * MANA_RECHARGE;
            if (timer >= 1 && Mana < 100)
            {
                //if (Mana > 100 - MANA_RECHARGE) Mana = 100;
                //else Mana++;
                Mana++;
                timer = 0;
            }        
        }       

        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 &&
                motionAngle <= MathHelper.PiOver4)
                SpriteAnimation.CurrentAnimationName = "Right";

            else if (motionAngle >= MathHelper.PiOver4 &&
                motionAngle <= 3f * MathHelper.PiOver4)
                SpriteAnimation.CurrentAnimationName = "Down";

            else if (motionAngle <= -MathHelper.PiOver4 &&
                motionAngle >= -3f * MathHelper.PiOver4)
                SpriteAnimation.CurrentAnimationName = "Up";

            else
                SpriteAnimation.CurrentAnimationName = "Left";
        }

        #endregion //Update

        public override void Draw()
        {
            if (!   (stopwatch.IsRunning && (stopwatch.Elapsed.Milliseconds / 100) % 2 == 0))
                base.Draw();
        }

        #region Input

        private void InputCheck()
        {
            if (currentControlScheme != Globals.controlScheme) // change control scheme only when the setting is changed
            {
                currentControlScheme = Globals.controlScheme;
                CheckControlScheme();
            }

            Vector2 movement = Vector2.Zero;

            if (Globals.input.KeyPressed(MoveRight) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.DPadRight) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.LeftThumbstickRight))
            {
                movement.X = 1;
                Direction = FacingDirection.Right;
            }
            else if (Globals.input.KeyPressed(MoveLeft) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.DPadLeft) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.LeftThumbstickLeft))
            {
                movement.X = -1;
                Direction = FacingDirection.Left;
            }
            if (Globals.input.KeyPressed(MoveUp) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.DPadUp) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.LeftThumbstickUp))
            {
                movement.Y = -1;
                Direction = FacingDirection.Up;
            }
            else if (Globals.input.KeyPressed(MoveDown) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.DPadDown) ||
                Globals.input.ButtonPressed(PlayerIndex.One, Buttons.LeftThumbstickDown))
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

            if (Globals.input.KeyPressed(Keys.D1) || (Globals.red == 1 && Globals.blue == 0 && Globals.green == 0))
                selectedSpell = SelectedSpell.fire;
            else if (Globals.input.KeyPressed(Keys.D2) || (Globals.red == 0 && Globals.blue == 0 && Globals.green == 1))
                selectedSpell = SelectedSpell.wind;

            CastSpell();

            if (Globals.input.KeyJustPressed(Keys.I))
                Globals.I_AM_INVINCIBLE = !Globals.I_AM_INVINCIBLE;
            if (Globals.input.KeyJustPressed(Keys.U))
                Globals.UNLIMITED_MANA = !Globals.UNLIMITED_MANA;
        }

        private void CastSpell()
        {
            if (red || green || blue)
            {
                if (Globals.input.KeyJustPressed(SpellCast) ||
                    Globals.input.ButtonJustPressed(PlayerIndex.One, Buttons.RightTrigger))
                {
                    if (Globals.input.KeyPressed(CastLeft) ||
                        Globals.input.ButtonPressed(PlayerIndex.One, Buttons.RightThumbstickLeft))
                        Direction = FacingDirection.Left;
                    else if (Globals.input.KeyPressed(CastRight) ||
                        Globals.input.ButtonPressed(PlayerIndex.One, Buttons.RightThumbstickRight))
                        Direction = FacingDirection.Right;
                    else if (Globals.input.KeyPressed(CastUp) ||
                        Globals.input.ButtonPressed(PlayerIndex.One, Buttons.RightThumbstickUp))
                        Direction = FacingDirection.Up;
                    else if (Globals.input.KeyPressed(CastDown) ||
                        Globals.input.ButtonPressed(PlayerIndex.One, Buttons.RightThumbstickDown))
                        Direction = FacingDirection.Down;

                    if (selectedSpell == SelectedSpell.fire && red && (Mana >= FIRE_SPELL_MANA || Globals.UNLIMITED_MANA))
                    {
                        GameWorld.Spells.Add(new FireSpell(Direction, new Vector2(SpriteAnimation.Position.X - SpriteAnimation.Bounds.Width/2 + 24, SpriteAnimation.Position.Y - SpriteAnimation.Bounds.Height/2 + 24)) { originatingType = GameObjects.Spell.OriginatingType.Player });
                        Mana -= FIRE_SPELL_MANA;
                        Globals.fireFX.Play(Globals.FXVolume(), 0, 0);
                    }
                    else if (selectedSpell == SelectedSpell.wind && green && (Mana >= WIND_SPELL_MANA || Globals.UNLIMITED_MANA))
                    {
                        GameWorld.Spells.Add(new WindSpell(Direction, new Vector2(SpriteAnimation.Position.X - SpriteAnimation.Bounds.Width / 2 + 24, SpriteAnimation.Position.Y - SpriteAnimation.Bounds.Height / 2 + 24)) { originatingType = GameObjects.Spell.OriginatingType.Player });
                        Mana -= WIND_SPELL_MANA;
                    }
                    else if (selectedSpell == SelectedSpell.water && blue)
                    {
                        // reserved for water spell
                    }
                    else
                        Globals.lowManaFX.Play(Globals.FXVolume(), 0, 0); 
                }
            }
        }

        #endregion //Input

        #region Controls

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

        private void CheckControlScheme()
        {
            switch (currentControlScheme)
            {
                case 0:
                    SetKeys(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Space, Keys.A, Keys.D, Keys.W, Keys.S);
                    break;
                case 1:
                    SetKeys(Keys.A, Keys.D, Keys.W, Keys.S, Keys.Space, Keys.Left, Keys.Right, Keys.Up, Keys.Down);
                    break;
                case 2:
                    SetKeys(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Space, Keys.None, Keys.None, Keys.None, Keys.None);
                    break;
            }
        }

        #endregion //Controls

        #region Collision

        public override void Collision(GameObject o)
        {
            if (!stopwatch.IsRunning)
                if (o is Enemy)
                {
                    o.Collision(this);
                    stopwatch.Start();

                    if (o is FireEnemy)
                    {
                        EnemyCollision(o.Direction);
                        Health -= 5;
                    }
                    else if (o is WindEnemy)
                    {
                        EnemyCollision(o.Direction);
                        Health -= 5;
                    }
                    else if (o is FireBoss)
                    {
                        EnemyCollision(o.Direction);
                        Health -= 10;
                    }
                    Globals.PhitFX.Play(Globals.FXVolume(), 0, 0);
                }
        }

        /// <summary>
        /// Push the player back in direction enemy faces
        /// </summary>
        /// <param name="Direction">The direction enemy is facing</param>
        private void EnemyCollision(FacingDirection Direction)
        {
            Rectangle futurePlayerRectangle = SpriteAnimation.Bounds;//rectangle we'll use to test

 
            /* This method is pretty dense, but I assure you it is light on resources.
             * First, we adjust where the player will be (if collision does occur)
             * then we cycle through all the clips in the clip map
             * if the clip intersects where the player will be,
             * we adjust the player to next the clip.
             * if no intersection was found, move the player the full length.
             */

            switch (Direction)
            {
                case FacingDirection.Left:
                    futurePlayerRectangle.X -= 20;
                    foreach (Rectangle clip in GameWorld.ClipMap.Values)
                        if (clip.Intersects(futurePlayerRectangle))
                            //if (clip.Right > SpriteAnimation.Position.X - 20)
                            {
                                SpriteAnimation.Position.X = clip.Right + 1;
                                return;
                            }
                    SpriteAnimation.Position.X -= 20;
                    break;
                case FacingDirection.Right:
                    futurePlayerRectangle.X += 20;
                    foreach (Rectangle clip in GameWorld.ClipMap.Values)
                        if (clip.Intersects(futurePlayerRectangle))
                            //if (clip.Left < SpriteAnimation.Position.X + 20)
                            {
                                SpriteAnimation.Position.X = clip.Left - 1;
                                return;
                            }
                    SpriteAnimation.Position.X += 20;
                    break;
                case FacingDirection.Up:
                    futurePlayerRectangle.Y -= 20;
                    foreach (Rectangle clip in GameWorld.ClipMap.Values)
                        if (clip.Intersects(futurePlayerRectangle))
                            //if (clip.Bottom > SpriteAnimation.Position.Y - 20)
                            {
                                SpriteAnimation.Position.Y = clip.Bottom + 1;
                                return;
                            }
                    SpriteAnimation.Position.Y -= 20;
                    break;
                case FacingDirection.Down:
                    futurePlayerRectangle.Y += 20;
                    foreach (Rectangle clip in GameWorld.ClipMap.Values)
                        if (clip.Intersects(futurePlayerRectangle))
                            //if (clip.Top < SpriteAnimation.Position.Y + 20)
                            {
                                SpriteAnimation.Position.Y = clip.Top - 1;
                                return;
                            }
                    SpriteAnimation.Position.Y += 20;
                    break;
            }
        }

        #endregion //Collision
    }

}
