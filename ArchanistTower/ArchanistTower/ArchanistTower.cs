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
            tileMap.CollisionLayer = CollisionLayer.FromFile("Content/Layers/Collision.layer");

            sprite = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/thf4"));
            sprite.OriginOffset = new Vector2(16, 32);

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

                motion = CheckCollisionForMotion(motion, sprite);

                sprite.Position += motion * sprite.Speed; //comment out for tile based movement
                UpdateSpriteAnimation(motion);
                sprite.IsAnimating = true;

                CheckForUnwalkableTimes(sprite);

                
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

        private void CheckForUnwalkableTimes(AnimatedSprite sprite)
        {
            Point spriteCell = Engine.ConvertPositionToCell(sprite.Center);

            Point? upLeft = null, up = null, upRight = null,
                left = null, right = null, downLeft = null,
                down = null, downRight = null;

            if (spriteCell.Y > 0)
                up = new Point(spriteCell.X, spriteCell.Y - 1);

            if (spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                down = new Point(spriteCell.X, spriteCell.Y + 1);

            if (spriteCell.X > 0)
                left = new Point(spriteCell.X - 1, spriteCell.Y);

            if (spriteCell.X < tileMap.CollisionLayer.Width - 1)
                right = new Point(spriteCell.X + 1, spriteCell.Y);

            if (spriteCell.X > 0 && spriteCell.Y > 0)
                upLeft = new Point(spriteCell.X - 1, spriteCell.Y - 1);

            if (spriteCell.X < tileMap.CollisionLayer.Width - 1 && spriteCell.Y > 0)
                upRight = new Point(spriteCell.X + 1, spriteCell.Y - 1);

            if (spriteCell.X > 0 && spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                downLeft = new Point(spriteCell.X - 1, spriteCell.Y + 1);

            if (spriteCell.X < tileMap.CollisionLayer.Width - 1 &&
                spriteCell.Y < tileMap.CollisionLayer.Height - 1)
                downRight = new Point(spriteCell.X + 1, spriteCell.Y + 1);


            if (up != null && tileMap.CollisionLayer.GetCellIndex(up.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(up.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }
            if (down != null && tileMap.CollisionLayer.GetCellIndex(down.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(down.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.Y = 
                        down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
                }
            }
            if (left != null && tileMap.CollisionLayer.GetCellIndex(left.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(left.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = spriteCell.X * Engine.TileWidth;
                }
            }
            if (right != null && tileMap.CollisionLayer.GetCellIndex(right.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X =
                        right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
                }
            }
            if (upLeft != null && tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                Rectangle spriteRect = sprite.Bounds;
                if(cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }
            if (upRight != null && tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(upRight.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X =
                        right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
                    sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }
            if (downLeft != null && tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downLeft.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = spriteCell.X * Engine.TileWidth;
                    sprite.Position.Y =
                        down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
                }
            }
            if (downRight != null && tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(downRight.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X =
                        right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
                    sprite.Position.Y =
                        down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
                }
            }

            
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

        private Vector2 CheckCollisionForMotion(Vector2 motion, AnimatedSprite sprite)
        {
            Point cell = Engine.ConvertPositionToCell(sprite.Origin);

            int colIndex = tileMap.CollisionLayer.GetCellIndex(cell);

            if (colIndex == 2)
                return motion * .2f;

            return motion;
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
