using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils;

namespace SolarConflict.CardGame
{
    public class CardDeck
    {
        private Random _random;
        private List<Card> _cards;

       // Queue<Card> _cards;

        public CardDeck()
        {
            _random = new Random();
            _cards = new List<Card>();
            InitDeck();
        }
        
        //Creates a sheffeld deck
        public void InitDeck()
        {
            String textureID = "card";
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 13; i++)
                {
                    _cards.Add(new Card(i, textureID));
                }
            }
            Shuffle();
        }

        public void Shuffle()
        {
            FMath.Shuffle<Card>(_cards, _random);
        }

        public Card PullCard()
        {
            Card card = null;
            if(_cards.Count > 0)
            {
                card = _cards[0];
                _cards.RemoveAt(0);
            }
            return card;
        }




    }
}
