using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc
{
    /// <summary>
    /// This system will add ship to your fleet
    /// </summary>
    [Serializable]
    public class AddShipToFleetSystem : AgentSystem
    {
        public IEmitter Emitter;
        public ControlSignals Activation;

        public AddShipToFleetSystem()
        { }

        public AddShipToFleetSystem(ControlSignals activation, IEmitter emitter)
        {
            Activation = activation;
            Emitter = emitter;
        }

        public AddShipToFleetSystem(ControlSignals activation, String emitterID)
        {
            Activation = activation;
            Emitter = ContentBank.Inst.GetEmitter(emitterID);
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if (agent.Lifetime % 30 == 0 && ( (agent.ControlSignal & Activation) > 0 || tryActivate))
            {

                Faction faction = gameEngine.GetFaction(agent.FactionType);
                FleetSystem.FleetSlot freeSlot = faction.MothershipHanger?.FindFreeSlot(); //TODO: add needed size;
                if (freeSlot != null)
                {
                    GameObject gameobject = Emitter.Emit(gameEngine, agent, agent.GetFactionType(), initPosition, agent.Velocity, initRotation);
                    freeSlot.Agent = gameobject as Agent; //change it 
                    AgentLoadout loadout = Emitter as AgentLoadout;
                    if (loadout != null)
                    {
                        gameEngine.GetFaction(agent.FactionType).AddHull(loadout.AgentID); //Try
                    }
                    return true;
                }
            }
            return false;
        }



        public override AgentSystem GetWorkingCopy()
        {
            // return MemberwiseClone() as AgentSystem;
            //Maybe this
            return this;
        }
    }
}
