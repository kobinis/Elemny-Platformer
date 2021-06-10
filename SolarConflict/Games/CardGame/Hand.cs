using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.CardGame
{
    public class Hand
    {
        public int MaxHandSize { get; private set; }
        public List<Card> Cards { get; private set; }

        public Hand() 
        {
            MaxHandSize = 5;  //
            Cards = new List<Card>();
        }

        public bool AddCard(Card card)
        {
            if (Cards.Count() < MaxHandSize)
            {
                Cards.Add(card);
                return true;
            }
            return false;
        }        
    }
}
