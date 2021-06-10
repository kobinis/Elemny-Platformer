using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    [Serializable]
    class SystemHolder : AgentSystem
    {
        public AgentSystem system;
        public Vector2 position;
        public float rotation;
        public bool FixToCenter; 

        public SystemHolder(AgentSystem system, Vector2 position, float rotation)
        {
            this.system = system;
            this.position = position;
            this.rotation = rotation;
        }

        public override bool Update(Agent agent, GameEngine engine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            if(FixToCenter)
            {
                initPosition = agent.Position;
            }

            Vector2 pos = new Vector2(initPosition.X + position.X * agent.Heading.X - position.Y * agent.Heading.Y, initPosition.Y + position.X * agent.Heading.Y + position.Y * agent.Heading.X);
            return system.Update(agent, engine, pos, initRotation + rotation, tryActivate);            
        }        

        public override AgentSystem GetWorkingCopy()
        {
            SystemHolder systemHolderClone = (SystemHolder)MemberwiseClone();
            systemHolderClone.system = system.GetWorkingCopy();
            return systemHolderClone;
        }

        public override float GetCooldown()
        {
            return system.GetCooldown();
        }

        public override float GetCooldownTime()
        {
            return system.GetCooldownTime();
        }
    }
}
