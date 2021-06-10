using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Games.Monopoly
{
    public abstract class Tile
    {
        public abstract void Land(Player player);
        public virtual Player GetOwner()
        {
            return null;
        }

    }
}
