using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.CardGame
{
    public class CardGameEngine
    {
        public CardDeck PlayerDeck;
        public CardDeck EnemyDeck;
        public Card EnemyCard;
        public Hand Hand;
        public Enemy Enemy;
        public Player Player;
        
        public CardGameEngine()
        {
            PlayerDeck = new CardDeck();
            Hand = new Hand();
            Enemy = new Enemy();
            Player = new Player();
            EnemyDeck = new CardDeck();

            for (int i = 0; i < 5; i++)
            {
                PullPlayerCard();
            }

            PullEnemyCard();
        }

        public void PullEnemyCard()
        {
            EnemyCard = EnemyDeck.PullCard();
        }

        public void PullPlayerCard()
        {
            Card card = PlayerDeck.PullCard();
            Hand.AddCard(card); //if deck is not empty and hand is not full, if hand is full burn card
        }

        public void PlayCard(int i)
        {
            if(Hand.Cards.Count > i)
            {
                var playerCard = Hand.Cards[i];

                Hand.Cards.RemoveAt(i);

                if(EnemyCard.Value > playerCard.Value)
                {
                    Player.Hitpoints -= 2;
                }

                if (EnemyCard.Value < playerCard.Value)
                {
                    Enemy.Hitpoints -= 2;
                }

                PullEnemyCard();
                PullPlayerCard();
            }
            
        }

        

        public bool HasGameEnded()
        {
            return Player.Hitpoints <=0 || Enemy.Hitpoints<=0; //or no more cards
        }
    }
}
