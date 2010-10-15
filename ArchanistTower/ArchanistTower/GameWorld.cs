using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using ArchanistTower.GameObjects;

namespace ArchanistTower
{
    /// <summary>
    /// This is the class where the all of the game information 
    /// will be kept and delt with. 
    /// All updates and draws for the main game will happen here.
    /// </summary>
     
   public class GameWorld
    { 
        public List<GameObject> WorldObjects;

        public void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in WorldObjects)
                gameObject.Update(gameTime);
        }

        public void Draw()
        {
            foreach (GameObject gameObject in WorldObjects)
                gameObject.Draw();
        }
    }
}
