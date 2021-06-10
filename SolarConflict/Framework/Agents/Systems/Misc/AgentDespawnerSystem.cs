using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    [Serializable]
    public class AgentDespawner : AgentSystem
    {
        public IEmitter Effect;
        public int MaxLifetime;
        //public float NeededRangeFromPlayer;
        public bool MustBeOffScreen;

        public AgentDespawner()
        {
            MaxLifetime = int.MaxValue;
            //NeededRangeFromPlayer = 0;            
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if (agent.Lifetime >= MaxLifetime && (!MustBeOffScreen || gameEngine.Camera.IsOnScreen(agent.Position, agent.Size)))
            {
                Effect?.Emit(gameEngine, agent, agent.GetFactionType(), initPosition, agent.Velocity, initRotation);
                agent.IsActive = false;
                return true;
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone(); //Can also be this;
        }
    }
}
