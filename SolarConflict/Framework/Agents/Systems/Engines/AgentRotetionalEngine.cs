using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    /// <summary>
    /// AgentRotetionalEngine - use to rotate the agent
    /// </summary>
   [Serializable]
    public class AgentRotationEngine : AgentSystem //TODO: change name
    {       
        public float RotationForce;

        public AgentRotationEngine(float rotationForce)
        {
            this.RotationForce = rotationForce;
        }

        public AgentRotationEngine(): this(0.1f)
        {
        }

        public override bool Update(Agent agent, GameEngine engine, Vector2 initPosition, float initRotation, bool tryActivate)
        {            
            Vector2 diraction = agent.analogDiractions[0];

            if (diraction.LengthSquared() > 0.1f)
            {
                float rotationSpeed = RotationForce / agent.RotationMass;
                float angle = (float)Math.Atan2(diraction.Y, diraction.X);
                float angleDiff = (float)FMath.AngleDiff(angle , agent.Rotation);
                if (Math.Abs(angleDiff) <= rotationSpeed)
                {
                    agent.Rotation = angle;
                }
                else
                {
                    agent.Rotation += rotationSpeed * (float)FMath.DegSign(angle - agent.Rotation);
                }
            }

            return false;
        }        

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentRotationEngine)MemberwiseClone();           
        }
    }
}
