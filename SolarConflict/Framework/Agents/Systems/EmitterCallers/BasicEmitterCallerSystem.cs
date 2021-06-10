using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers
{
    /// <summary>
    /// Calls an Emitter from an agent
    /// </summary>
    [Serializable]
    public class BasicEmitterCallerSystem : AgentSystem
    {
        public IEmitter Emitter;
        public ControlSignals Activation;
        public int CooldownTime;
        private int cooldown;

        public BasicEmitterCallerSystem()
        { }

        public BasicEmitterCallerSystem(ControlSignals activation, IEmitter emitter, int cooldownTime = 0)
        {
            CooldownTime = cooldownTime;
            Activation = activation;
            Emitter = emitter;
        }

        public BasicEmitterCallerSystem(ControlSignals activation, String emitterID)
        {
            Activation = activation;
            Emitter = ContentBank.Inst.GetEmitter(emitterID);
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if (cooldown <= 0)
            {
                if ((agent.ControlSignal & Activation) > 0 || tryActivate)
                {
                    cooldown = CooldownTime;
                    Emitter.Emit(gameEngine, agent, agent.GetFactionType(), initPosition, agent.Velocity, initRotation);
                    return true;
                }
            }
            else
            {
                cooldown--;
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return MemberwiseClone() as AgentSystem;
        }
    }
}
