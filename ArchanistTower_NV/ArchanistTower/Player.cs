using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            
            playerSprite.OriginOffset = new Vector2(16, 32);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw()
        {
            levelList[currentLevel].Draw(Globals.spriteBatch);
        }


        private void AddMapsToLevel()
        {
            levelList[0].AddMap(Globals.content.Load<Map>("Levels/TestMap/TestMap"));
            levelList[0].StartMap = 0;
        }

    }
}
