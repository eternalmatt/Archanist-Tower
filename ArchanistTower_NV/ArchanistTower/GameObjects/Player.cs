using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ArchanistTower.Screens;

namespace ArchanistTower.GameWorld
{
    class Player : GameObject
    {
        protected Keys MoveLeft;
        protected Keys MoveRight;
        protected Keys MoveUp;
        protected Keys MoveDown;
        protected Keys SpellCast;

        protected bool red;
        protected bool blue;
        protected bool green;

        enum SelectedSpell
        {
            fire,
            wind,
            water
        }
        
        public Player()
        {

        }

        public override void Initialize()
        {
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

            SetKeys(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Space);
            
            base.Initialize();
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
    }

}
