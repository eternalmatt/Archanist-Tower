using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArchanistTower
{
    class Player
    {
        int currentLevel = 0;
        List<Level> levelList = new List<Level>();

        public static AnimatedSprite playerSprite;

        public Player()
        { }

        public void Initialize()
        {
            Level testLevel = new Level();
            levelList.Add(testLevel);

            playerSprite = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/Player/man1"));

            FrameAnimation up = new FrameAnimation(2, 32, 32, 0, 0);
            up.FramesPerSecond = 10;
            playerSprite.Animations.Add("Up", up);

            FrameAnimation down = new FrameAnimation(2, 32, 32, 64, 0);
            down.FramesPerSecond = 10;
            playerSprite.Animations.Add("Down", down);

            FrameAnimation left = new FrameAnimation(2, 32, 32, 128, 0);
            left.FramesPerSecond = 10;
            playerSprite.Animations.Add("Left", left);

            FrameAnimation right = new FrameAnimation(2, 32, 32, 192, 0);
            right.FramesPerSecond = 10;
            playerSprite.Animations.Add("Right", right);

            playerSprite.CurrentAnimationName = "Down";
        }        

        public void LoadContent()
        {
            AddMapsToLevel();

            //Load Player Sprites
            levelList[currentLevel].MapStartPosition(playerSprite);
            playerSprite.OriginOffset = new Vector2(16, 32);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            Vector2 motion = Vector2.Zero;

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
                //Add Check here for motion effects(i.e. ice)
                //motion = 

                playerSprite.Position += motion * playerSprite.Speed;
                UpdateSpriteAnimation(motion);
                playerSprite.IsAnimating = true;

                playerSprite = levelList[currentLevel].CollisionCheck(playerSprite);
            }
            else
                playerSprite.IsAnimating = false;

            playerSprite.ClampToArea(
                levelList[currentLevel].MapWidthInPixels,
                levelList[currentLevel].MapHeightInPixels);

            playerSprite.Update(gameTime);

            Globals.camera.LockToTarget(playerSprite, Globals.ScreenWidth, Globals.ScreenHeight);

            Globals.camera.ClampToArea(
                levelList[currentLevel].MapWidthInPixels - Globals.ScreenWidth,
                levelList[currentLevel].MapHeightInPixels - Globals.ScreenHeight);

        }

        public void Draw()
        {
            Globals.spriteBatch.Begin(
                    SpriteBlendMode.AlphaBlend,
                    SpriteSortMode.Immediate,
                    SaveStateMode.None,
                    Globals.camera.TransformMatrix);

            levelList[currentLevel].Draw(Globals.spriteBatch);
            //Globals.shader.Draw();
            playerSprite.Draw(Globals.spriteBatch);

            Globals.spriteBatch.End();
        }


        private void AddMapsToLevel()
        {
            levelList[0].AddMap(Globals.content.Load<Map>("Levels/TestMap/TestMap"));
            levelList[0].StartMap = 0;
        }

        //private Vector2 CheckCollision()
        //{

        //}
    
        private void UpdateSpriteAnimation(Vector2 motion)
        {
            float motionAngle = (float)Math.Atan2(motion.Y, motion.X);

            if (motionAngle >= -MathHelper.PiOver4 &&
                motionAngle <= MathHelper.PiOver4)
            {
                playerSprite.CurrentAnimationName = "Right";
                //motion = new Vector2(1f, 0f);  //These lines restrict diagonal movement
            }
            else if (motionAngle >= MathHelper.PiOver4 &&
                motionAngle <= 3f * MathHelper.PiOver4)
            {
                playerSprite.CurrentAnimationName = "Down";
                //motion = new Vector2(0f, 1f);
            }
            else if (motionAngle <= -MathHelper.PiOver4 &&
                motionAngle >= -3f * MathHelper.PiOver4)
            {
                playerSprite.CurrentAnimationName = "Up";
                //motion = new Vector2(0f, -1f);
            }
            else
            {
                playerSprite.CurrentAnimationName = "Left";
                //motion = new Vector2(-1f, 0f);
            }
            //sprite.Position += motion * sprite.Speed; //must be after for tile based movement
        }
    }
}
