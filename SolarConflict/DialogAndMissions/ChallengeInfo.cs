using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Scenes
{
    /*     
    Add back button

    Input parameters:
    loadouts: - comma seperated AgentFactory Id's
    player_loadouts: - comma seperated AgentFactory Id's can be PlayerClone or a spesific loadout
    highscore: - the high score of this challange to notefiy if you betan the high score in the end
    general params:
        - background

    Output parameters:
    score
    victory
    General params:
       - time passed??

    Challanges types:
        Defend the core - wafes from the left and from the right


    */

    /// <summary>
    /// 
    /// </summary>     
    [Serializable]
    public class ChallengeInfo //All this shit can just be in activity param
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Sprite Icon { get; set; } 
        public string IconText { get; set; }
        public FactionType GiverFaction { get; set; }
        public Reward Reward { get; set; }

        public string ActivityID { get; set; }
        public Dictionary<string, string> ActivityParameters { get; private set; }

        public bool IsComplete { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        
        //Add Best Time

        public ChallengeInfo() //optional to pass activity param
        {
            ActivityParameters = new Dictionary<string, string>();
        }        

        public void OnChallengeEnd(ActivityParameters parameters, GameEngine gameEngine)
        {
            bool hasWon = parameters.ParamDictionary.ContainsKey("victory");
            //MetaWorld.Inst.GlobalMessages["arena"] = hasWon.ToString();
            if (hasWon && !IsComplete)
            {
                Reward.AddReward(gameEngine, GiverFaction);
                IsComplete = true;                
            }
        }
    }
}
