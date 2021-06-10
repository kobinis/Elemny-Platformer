using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Games.Monopoly
{
    public class Player
    {
        public Player(int index, int money)
        {
            Index = index;
            Money = money;
        }

        public int Index;
        public int Position;
        public int Money;

        public int AnimationPosition;

        public void Move(int delta)
        {
            //Event and animation
            Position = (Position + delta) % MonopolyGame.BOARD_LENGTH ;
            
        }
    }
}
