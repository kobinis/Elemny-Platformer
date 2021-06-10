using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems
{
    [Serializable]
    class FrictionSystem : AgentSystem
    {
        public float Inerita;

        public FrictionSystem(float inerita)
        {
            Inerita = inerita;
        }


        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            agent.Velocity *= Inerita;
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}
