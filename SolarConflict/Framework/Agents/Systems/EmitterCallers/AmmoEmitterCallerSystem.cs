using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.Agents.Systems.EmitterCallers
{
    /// <summary>
    /// Used for ammo weapons, calls the emitter of the ammo being consumed
    /// </summary>
    [Serializable]
    class AmmoEmitterCallerSystem:AgentSystem
    {
        public IEmitter Emitter;
        public ControlSignals Activation;
        public int CooldownTime;
        public ItemCategory AmmoTypeNeeded;
        public float Speed = 50;    

        private int cooldown;
        private Item ammoItem;
        

        public AmmoEmitterCallerSystem()
        { }

        public AmmoEmitterCallerSystem(ControlSignals activation, IEmitter emitter)
        {
            Activation = activation;
            Emitter = emitter;
        }

        public AmmoEmitterCallerSystem(ControlSignals activation, String emitterID)
        {
            Activation = activation;
            Emitter = ContentBank.Inst.GetEmitter(emitterID);
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            bool wasActive = false;
            Vector2 velocity = FMath.RotateVector(Vector2.UnitX, initRotation) * Speed;
            if (cooldown <= 0 &&( (agent.ControlSignal & Activation) > 0 || tryActivate))
            {
                if (AmmoTypeNeeded != ItemCategory.None && (ammoItem = CheckForAmmo(agent, AmmoTypeNeeded)) != null)
                {
                    cooldown = CooldownTime;
                    Emitter?.Emit(gameEngine, agent, agent.GetFactionType(), initPosition, agent.Velocity, initRotation);              
                    ammoItem.Profile.AmmoEmitter?.Emit(gameEngine, agent, agent.GetFactionType(), initPosition, agent.Velocity + velocity, initRotation);
                    //if consuming
                    ammoItem.Stack--;                   
                    wasActive = true;
                }
            }
            cooldown--;
            return wasActive;
        }

        private Item CheckForAmmo(Agent agent, ItemCategory ammoTypeNeeded)
        {
            if (agent.Inventory == null)
                return null;
            for (int i = 0; i < agent.Inventory.Items.Length; i++)
            {
                var item = agent.Inventory.Items[i];
                if (item != null && (item.Category & ammoTypeNeeded) >0 )
                {
                    return item;
                }
            }
            return null;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}
