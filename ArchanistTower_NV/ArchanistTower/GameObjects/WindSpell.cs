using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ArchanistTower.GameObjects
{
    public class WindSpell : Spell
    {

        public WindSpell(FacingDirection fd, Vector2 pPosition)
        {
            SpellOrigin = pPosition;
            Direction = fd;
        }
    }
}
