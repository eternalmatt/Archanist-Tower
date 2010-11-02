using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArchanistTower.GameObjects;
using Microsoft.Xna.Framework.Graphics;
namespace ArchanistTower.Screens
{
    class GameScreen : Screen
    {
        int currentLevel;

        List<Level> levels = new List<Level>();
        List<GameObject> gameObjects = new List<GameObject>();

        public int CurrnentLevel
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
