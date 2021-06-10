using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Games.Monopoly
{
    public class StartingPointTile : Tile
    {
        private int reward;

        public StartingPointTile()
        {
            reward = 100;
        }

        public override void Land( Player player)
        {
            player.Money += reward;
            //TODO: Event
        }
    }
}
