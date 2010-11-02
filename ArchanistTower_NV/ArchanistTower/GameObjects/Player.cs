<<<<<<< .mineusing System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using TileEngine;
using Microsoft.Xna.Framework.Graphics;

namespace ArchanistTower.GameObjects
{
    public class Player : GameObject
    {
        public static AnimatedSprite playerSprite;

        float x, y;

        public Player()
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();

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
            playerSprite = new AnimatedSprite(Globals.content.Load<Texture2D>("Sprites/thf4"));
            playerSprite.OriginOffset = new Vector2(16, 32);
        }

        public override void Update(GameTime gameTime)
        {

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

                motion = CheckCollisionForMotion(motion, playerSprite);

                playerSprite.Position += motion * playerSprite.Speed; //comment out for tile based movement
                UpdateSpriteAnimation(motion);
                playerSprite.IsAnimating = true;

                CheckForUnwalkableTimes(playerSprite);


            }
            else
            {
                playerSprite.IsAnimating = false;
            }

            playerSprite.ClampToArea(ArchanistTower.tileMap.GetWidthInPixels(), ArchanistTower.tileMap.GetHeightInPixels());

            playerSprite.Update(gameTime);

            ArchanistTower.camera.LockToTarget(playerSprite, Globals.ScreenWidth, Globals.ScreenHeight);

            ArchanistTower.camera.ClampToArea(
                ArchanistTower.tileMap.GetWidthInPixels() - Globals.ScreenWidth,
                ArchanistTower.tileMap.GetHeightInPixels() - Globals.ScreenHeight);

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

            if (spriteCell.Y < ArchanistTower.tileMap.CollisionLayer.Height - 1)
                down = new Point(spriteCell.X, spriteCell.Y + 1);

            if (spriteCell.X > 0)
                left = new Point(spriteCell.X - 1, spriteCell.Y);

            if (spriteCell.X < ArchanistTower.tileMap.CollisionLayer.Width - 1)
                right = new Point(spriteCell.X + 1, spriteCell.Y);

            if (spriteCell.X > 0 && spriteCell.Y > 0)
                upLeft = new Point(spriteCell.X - 1, spriteCell.Y - 1);

            if (spriteCell.X < ArchanistTower.tileMap.CollisionLayer.Width - 1 && spriteCell.Y > 0)
                upRight = new Point(spriteCell.X + 1, spriteCell.Y - 1);

            if (spriteCell.X > 0 && spriteCell.Y < ArchanistTower.tileMap.CollisionLayer.Height - 1)
                downLeft = new Point(spriteCell.X - 1, spriteCell.Y + 1);

            if (spriteCell.X < ArchanistTower.tileMap.CollisionLayer.Width - 1 &&
                spriteCell.Y < ArchanistTower.tileMap.CollisionLayer.Height - 1)
                downRight = new Point(spriteCell.X + 1, spriteCell.Y + 1);


            if (up != null && ArchanistTower.tileMap.CollisionLayer.GetCellIndex(up.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(up.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                }
            }
            if (down != null && ArchanistTower.tileMap.CollisionLayer.GetCellIndex(down.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(down.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.Y =
                        down.Value.Y * Engine.TileHeight - sprite.Bounds.Height;
                }
            }
            if (left != null && ArchanistTower.tileMap.CollisionLayer.GetCellIndex(left.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(left.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X = spriteCell.X * Engine.TileWidth;
                }
            }
            if (right != null && ArchanistTower.tileMap.CollisionLayer.GetCellIndex(right.Value) == 1)
            {
                Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                Rectangle spriteRect = sprite.Bounds;
                if (cellRect.Intersects(spriteRect))
                {
                    sprite.Position.X =
                        right.Value.X * Engine.TileWidth - sprite.Bounds.Width;
                }
            }
            /*            if (upLeft != null && tileMap.CollisionLayer.GetCellIndex(upLeft.Value) == 1)
                        {
                            Rectangle cellRect = Engine.CreateRectForCell(right.Value);
                            Rectangle spriteRect = sprite.Bounds;
                            if(cellRect.Intersects(spriteRect))
                            {
                                sprite.Position.X = spriteCell.X * Engine.TileWidth;
                                sprite.Position.Y = spriteCell.Y * Engine.TileHeight;
                            }
                        }*/
            if (upRight != null && ArchanistTower.tileMap.CollisionLayer.GetCellIndex(upRight.Value) == 1)
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
            if (downLeft != null && ArchanistTower.tileMap.CollisionLayer.GetCellIndex(downLeft.Value) == 1)
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
            if (downRight != null && ArchanistTower.tileMap.CollisionLayer.GetCellIndex(downRight.Value) == 1)
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

        private Vector2 CheckCollisionForMotion(Vector2 motion, AnimatedSprite sprite)
        {
            Point cell = Engine.ConvertPositionToCell(sprite.Origin);

            int colIndex = ArchanistTower.tileMap.CollisionLayer.GetCellIndex(cell);

            if (colIndex == 2)
                return motion * .2f;

            return motion;
        }

        public float getXPosition()
        {
            x = playerSprite.Position.X;
            return x;
        }

        public float getYPosition()
        {
            y = playerSprite.Position.Y;
            return y;
        }
       

        public override void Draw()
        {
            
            playerSprite.Draw(Globals.spriteBatch);
            


            base.Draw();
        }
    }
}
=======using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchanistTower.GameObjects
{
    class Player
    {
        ArchanistTower
    }
}
>>>>>>> .theirs