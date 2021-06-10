using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Games.Monopoly
{

    public class MonopolyGame
    {
        public const int BOARD_LENGTH = 24;
        public const int PLAYER_NUM = 2;
        //Board
        Tile[] tiles;
        Player[] players;
        public int CurrentPlayerType { get; private set; }
        public static Random Rand = new Random();

        public MonopolyGame()
        {
            InitBoard();
            InitPlayers();
            CurrentPlayerType = 0;            
        }

        public Player GetPlayer(int index)
        {
            return players[index];
        }

        private void InitBoard()
        {
            tiles = new Tile[BOARD_LENGTH];
            for (int i = 0; i < BOARD_LENGTH; i++)
            {
                if (i % 6 == 0)
                {
                    if (i == 0)
                        tiles[i] = new StartingPointTile();
                    else
                        tiles[i] = new RewardTile();
                }
                else
                    tiles[i] = new PropertyTile(i * 10);
                
            }
        }

        private void InitPlayers()
        {
            players = new Player[PLAYER_NUM];
            for (int i = 0; i < PLAYER_NUM; i++)
            {
                players[i] = new Player(i, 300);
            }
        }

        //Moves turn to the next player;
        private void NextPlayer()
        {
            CurrentPlayerType = (CurrentPlayerType + 1) % PLAYER_NUM;

        }

        public bool MakeTurn()
        {
            if (!CheckIfGameEnded())
            {
                int diceNum = Rand.Next(6) + 1;
                Player currentPlayer = GetPlayer(CurrentPlayerType);
                currentPlayer.Move(diceNum);
                tiles[currentPlayer.Position].Land(currentPlayer);
                NextPlayer();
                return false;
            }
            return true;
        }

        private bool CheckIfGameEnded() 
        {
            foreach (var item in players) //TODO: improve to more players
            {
                if (item.Money <= 0)
                    return true;
            }
            return false;
        }

        
    }
}
