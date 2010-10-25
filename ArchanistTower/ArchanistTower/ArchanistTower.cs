using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using TileEngine;

namespace ArchanistTower
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ArchanistTower : Microsoft.Xna.Framework.Game
    {
        TileMap tileMap = new TileMap();
        Camera camera = new Camera();

        List<AnimatedSprite> npcs = new List<AnimatedSprite>();
        AnimatedSprite sprite;
        SpriteFont font;
 
        public ArchanistTower()
        {
            Globals.Initialize(this);
        }

        protected override void Initialize()
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

            Random rand = new Random();

            foreach(AnimatedSprite s in npcs)
            {
                s.Animations.Add("Up", (FrameAnimation)up.Clone());
                s.Animations.Add("Down", (FrameAnimation)down.Clone());
                s.Animations.Add("Left", (FrameAnimation)left.Clone());
                s.Animations.Add("Right", (FrameAnimation)right.Clone());

                int animation = rand.Next(3);

                switch(animation)
                {
                    case 0:
                        s.CurrentAnimationName = "Up";
                        break;
                    case 1:
                        s.CurrentAnimationName = "Down";
                        break;
                    case 2:
                        s.CurrentAnimationName = "Left";
                        break;
                    case 3:
                        s.CurrentAnimationName = "Right";
                        break;
                }

                s.Position = new Vector2(
                    rand.Next(tileMap.GetWidthInPixels() - 32),
                    rand.Next(tileMap.GetHeightInPixels() - 32));
            }

            sprite.CurrentAnimationName = "Down";
        }

        protected override void LoadContent()
        {
            Globals.LoadContent();
            tileMap.Layers.Add(TileLayer.FromFile(Globals.content, "Content/Layers/Layer1.layer"));

            sprite = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/thf4"));

            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/man1")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/knt4")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/mst3")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/thf4")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/smr1")));
            npcs.Add(new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/nja2")));
            font = Globals.content.Load<SpriteFont>("Fonts/Arial");
            HUD.NewHUDItem(font, "Hello, World!", new Vector2(200, 200), Color.White);
        }
        protected override void UnloadContent()
        { }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (AnimatedSprite s in npcs)
                s.Update(gameTime);

            KeyboardState keyState = Keyboard.GetState();
            Vector2 motion = Vector2.Zero;

            //GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            //motion = new Vector2(gamePadState.ThumbSticks.Left.X, -gamePadState.ThumbSticks.Left.Y);

            if (keyState.IsKeyDown(Keys.W) || keyState.IsKeyDown(Keys.Up))
                motion.Y--;
            if (keyState.IsKeyDown(Keys.S) || keyState.IsKeyDown(Keys.Down))
                motion.Y++;
            if (keyState.IsKeyDown(Keys.A) || keyState.IsKeyDown(Keys.Left))
                motion.X--;
            if (keyState.IsKeyDown(Keys.D) || keyState.IsKeyDown(Keys.Right))
                motion.X++;

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                sprite.Position += motion * sprite.Speed; //comment out for tile based movement
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

                sprite.IsAnimating = true;
            }
            else
            {
                sprite.IsAnimating = false;
            }

            sprite.ClampToArea(tileMap.GetWidthInPixels(), tileMap.GetHeightInPixels());

            sprite.Update(gameTime);
            
            camera.LockToTarget(sprite, Globals.ScreenWidth, Globals.ScreenHeight);

            camera.ClampToArea(
                tileMap.GetWidthInPixels() - Globals.ScreenWidth,
                tileMap.GetHeightInPixels() - Globals.ScreenHeight);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Globals.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            tileMap.Draw(Globals.spriteBatch, camera);

            Globals.spriteBatch.Begin(
                SpriteBlendMode.AlphaBlend,
                SpriteSortMode.Texture,
                SaveStateMode.None,
                camera.TransformMatrix);
            sprite.Draw(Globals.spriteBatch);

            foreach (AnimatedSprite s in npcs)
                s.Draw(Globals.spriteBatch);
            HUD.Draw(Globals.spriteBatch);
            Globals.spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
