using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Agents;

namespace SolarConflict
{
    /// <summary>
    /// Hold data for Character
    /// </summary>
    [Serializable]
    public class CharacterSystem : AgentSystem //TODO: replace with charechter bank hold charcter id num in bank
    {
        public Character Character { get; set; }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {         
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }        
    }
}

