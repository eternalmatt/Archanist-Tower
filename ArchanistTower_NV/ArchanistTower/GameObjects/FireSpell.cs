using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.GameObjects
{
    public class FireSpell : Spell
    {

        public FireSpell(FacingDirection fd, Vector2 pPosition)
        {
            SpellOrigin = pPosition;
            Direction = fd;
        }
    }
}
