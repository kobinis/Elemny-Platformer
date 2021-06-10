using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents;
using SolarConflict.Framework.Scenes;

namespace SolarConflict
{
    /// <summary>
    /// Holds a list of challenges
    /// </summary>
    [Serializable]
    public class ChallengesSystem : AgentSystem
    {
        public List<ChallengeInfo> ChallengeList { get; set; }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this; //Or MemberwiseClone
        }
    }
}

