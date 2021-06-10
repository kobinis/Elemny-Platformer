using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict
{
    [Serializable]
    public class ReputationUpdateSystem : AgentSystem //TODO: can take delta from the cost of the hull
    {
      //  public ControlSignalsMask Activation;
        public float ReputationDelta { get; set; }

        public ReputationUpdateSystem(float delta = -0.1f)
        {
          //  Activation.Signals = ControlSignals.OnDestroyed;
            ReputationDelta = delta;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if(agent.FactionType != 0 && (agent.ControlSignal & ControlSignals.OnDestroyed) > 0)
            {                
                if (agent.lastDamagingObjectToCollide != null && agent.lastDamagingObjectToCollide.GetFactionType() != 0)
                {
                    FactionType enemyFaction = agent.lastDamagingObjectToCollide.GetFactionType();
                    gameEngine.GetFaction(agent.GetFactionType()).ChangeRelationToFaction(gameEngine, enemyFaction, ReputationDelta);
                    return true;
                }                
            }
            return false;
        }        

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }

        
    }
}
