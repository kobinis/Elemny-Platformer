using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Games.Monopoly
{
    public class RewardTile : Tile
    {
        public int BaseReward { get; private set; }
        private float[] rewardTable = { 0.2f, 1.8f, 0.8f, 1.2f, 0.8f, 1.2f, 0.6f, 1.4f, 0.6f, 1.4f, 1,1,1,1,1,1,1,1,1,1};

        public RewardTile()
        {
            BaseReward = 50;
        }       

        public override void Land( Player player)
        {
            int reward = CalculateReward();
            player.Money += reward;
            //TODO: reward callback and animation
        }

        public int CalculateReward()
        {            
            return (int)(BaseReward * rewardTable[MonopolyGame.Rand.Next(rewardTable.Length)] ); 
        }
    }
}
