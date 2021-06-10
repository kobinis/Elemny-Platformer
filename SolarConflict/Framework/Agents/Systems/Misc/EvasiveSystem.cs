using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;

namespace SolarConflict.Framework.Agents.Systems.Misc
{
    /// <summary>
    /// EvasiveSystem - helps the agent evaide incoming projectiles, 
    /// When activated will apply a force in an ortogonal dirction to the projectile velocity
    /// </summary>
    [Serializable]
    class EvasiveSystem : AgentSystem
    {
        public float Force;
        public float SpeedLimit = 100;

        public IEmitter trailEmitter;

        public EvasiveSystem(float force = 1, float speedLimit = 100)
        {
            Force = force;
            SpeedLimit = speedLimit;
            trailEmitter = ContentBank.Inst.GetEmitter("ProjEngineTrail");
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if(tryActivate)
            {
                if(agent.ClosesetDanger != null && agent.ClosesetDanger.CollisionInfo.IsDamaging)
                {
                    
                    GameObject projectile = agent.ClosesetDanger;
                    Vector2 diff = agent.Position - projectile.Position;
                    Vector2 velocity = projectile.Velocity;
                    float velAngle = (float)Math.Atan2(velocity.Y, velocity.X);
                    float posAngle = (float)Math.Atan2(diff.Y, diff.X);
                    float degSign = FMath.DegSign(velAngle - posAngle);
                    float newAngle = velAngle - degSign * MathHelper.PiOver2;
                    Vector2 dir = FMath.ToCartesian(1, newAngle);
                    agent.ApplyForce(dir*Force, SpeedLimit);
                    trailEmitter?.Emit(gameEngine, agent, 0, initPosition, dir*Force, initRotation, 0);                    
                }
                return true;
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }

    }
}
