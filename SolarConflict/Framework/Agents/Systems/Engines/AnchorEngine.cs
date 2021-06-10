using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Agents.Systems.Engines
{
    /// <summary>
    /// Pushes agent back to place
    /// </summary>
    [Serializable]
    public class AnchorEngine : AgentSystem
    {
        public Vector2 AnchorPosition;
        

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if(tryActivate || AnchorPosition == Vector2.Zero)
            {
                AnchorPosition = agent.Position;
                return true;
            }

            Vector2 diff = AnchorPosition - agent.Position;
            agent.ApplyForce(diff, 20);            
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            AnchorEngine clone = MemberwiseClone() as AnchorEngine;
            clone.AnchorPosition = Vector2.Zero;
            return clone;
        }
    }
}
