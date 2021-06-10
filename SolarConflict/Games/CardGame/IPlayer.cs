using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.CardGame
{
    public interface IPlayer
    {
        int Hitpoints { get; set; }
        string ImageID { get; set; }
    }
}
