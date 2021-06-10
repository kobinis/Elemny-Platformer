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
    public class PhysicalEngineSystem : AgentSystem
    {
        public ActivationCheck activationCheck;
        public float force;
        public float maxSpeed;

        public IEmitter trailEmitter;
        public float trailSpeed;
        public Color color;

        // private bool isActive;

        public PhysicalEngineSystem(float force, float maxSpeed)
        {
            color = Color.White;
            activationCheck = new ActivationCheck();
            this.force = force;
            this.maxSpeed = maxSpeed;
            trailEmitter = ContentBank.Inst.GetEmitter("ProjEngineTrail"); //check if 
            trailSpeed = 3;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            if (activationCheck.Check(agent, tryActivate) )
            {
                activationCheck.DrainCost(agent);
                Vector2 initRotVector = new Vector2((float)Math.Cos(initRotation), (float)Math.Sin(initRotation));
                Vector2 rotatedForce = -FMath.RotateVector(initRotVector, Vector2.UnitX);
                float refRotationSpeed = (float)gameEngine.Rand.Next(2) - 1f;
                agent.ApplyForce(rotatedForce * force, initPosition, maxSpeed);
                if (!agent.IsCloaked)
                    trailEmitter?.Emit(gameEngine, agent, 0, initPosition, agent.Velocity - trailSpeed * rotatedForce, initRotation, refRotationSpeed);
                return true;
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone();
        }
    }
}

