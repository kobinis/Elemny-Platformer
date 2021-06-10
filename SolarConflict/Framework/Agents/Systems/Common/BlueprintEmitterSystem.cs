
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers
{
    /// <summary>
    /// Drops ship blueprint //TODO: can be replaced by an asimple emitter with costume activation
    /// </summary>
    [Serializable]
    public class BlueprintEmitterSystem : AgentSystem
    {
        public IEmitter Emitter;        

        public BlueprintEmitterSystem(IEmitter emitter)
        {
            Emitter = emitter;
        }

        public BlueprintEmitterSystem(string emitterID):this(ContentBank.Inst.GetEmitter(emitterID))
        {            
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if((agent.ControlSignal & ControlSignals.OnDestroyed) > 0 && (agent.lastDamagingObjectToCollide == null || agent.lastDamagingObjectToCollide.GetFactionType() == FactionType.Player))
            {
                Emitter.Emit(gameEngine, agent, agent.GetFactionType(), initPosition, agent.Velocity, initRotation);
            }            
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}
