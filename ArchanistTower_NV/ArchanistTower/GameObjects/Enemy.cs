using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArchanistTower.GameObjects
{
    public class Enemy : GameObject
    {

        /*This isnt the place to place the code for a specific enemy.  This needs to be the 
         * general methods and properties that all enemies will have.
         */

        public FacingDirection Direction { get; set; }


    }
}
