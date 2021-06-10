using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Games.Monopoly
{
    public class PropertyTile : Tile
    {
        private Player owner; //We can also hold the actual owner
        public int Cost { get; private set; }
        public int Rent;

        public PropertyTile(int cost)
        {
            Cost = cost;
            Rent = cost;
        }

        public override void Land( Player player)
        {
            if (owner == null)
            {
                //try to buy
                if (Cost < player.Money)
                {
                    player.Money -= Cost;
                    owner = player;
                }
            }
            else
            {
                //PayRent
                if (owner.Index != player.Index) 
                {
                    player.Money -= Rent;
                    owner.Money += Rent;
                    //Pay animation
                }                
            }
        }

        public override Player GetOwner()
        {
            return owner;
        }
    }
}
