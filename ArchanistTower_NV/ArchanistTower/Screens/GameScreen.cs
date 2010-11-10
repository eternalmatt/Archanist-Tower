﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArchanistTower.GameWorld;

namespace ArchanistTower.Screens
{
    public class GameScreen : Screen
    {
        public static GameWorld gameWorld;
        int currentLevel;
        public static List<Level> levels = new List<Level>();


        public int CurrentLevel
        {
            get { return currentLevel; }
            set
            {
                currentLevel = (int)MathHelper.Clamp(value, 0, levels.Count - 1);
            }
        }

        public GameScreen(Game game) :
            base(game, Globals.spriteBatch)
        {   }

        public override void Initialize()
        {
            base.Initialize();
        }

    }
}
