using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.CardGame
{
    public class Player:IPlayer
    {
        public int Hitpoints { get; set; }
        public string ImageID { get; set; }

        public Player()
        {
            Hitpoints = 100;
        }
    }
}
