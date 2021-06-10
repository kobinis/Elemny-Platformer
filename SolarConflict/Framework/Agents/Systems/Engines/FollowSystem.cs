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
    public class FollowSystem : AgentSystem
    {
        public ActivationCheck activationCheck;
        public float force;
        public float maxSpeed;

        public IEmitter trailEmitter;
        public float trailSpeed;
        public Color color;

        // private bool isActive;

        public FollowSystem(float force, float maxSpeed)
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
            var goal = gameEngine.PlayerAgent; //agent.GetTarget(gameEngine, TargetType.Goal);
            
            if(goal != null)
            {
                if(GameObject.DistanceFromEdge(agent, goal) > 800)
                {
                    agent.ApplyForce(-(agent.Position - goal.Position).Normalized() * 20, 80);
                }
            }
                                    
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return (FollowSystem)MemberwiseClone();
        }
    }
}
