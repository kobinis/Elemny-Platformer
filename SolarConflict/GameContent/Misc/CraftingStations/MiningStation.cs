using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.CraftingStations
{
    class MiningStation
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("MiningStation", 1000);//Size= 100
            var agent = ShipQuickStart.Make(data);
            agent.gameObjectType |= GameObjectType.CraftingStation;
            agent.gameObjectType &= ~GameObjectType.Ship;
            agent.CraftingStationType = CraftingStationType.Mining;
            agent.isControllable = false;
            agent.Inventory = null;
            agent.control.controlAi = null;
            agent.Name = "Mining Station";

            //Basic gear
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.InteractionSystem = new ActivitySwitcherSystem("CraftingActivity");


            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter("MiningStationKit");
            groupEmitter.AddEmitter("CargoDropEmitter");
            groupEmitter.AddEmitter("AgentSlotDropEmitter");
            groupEmitter.AddEmitter("RemoveGameObjectEmitter");

            //Add remove cargo
            //Remove slot items items

            EmitterCallerSystem minedEmitter = new EmitterCallerSystem(ControlSignals.OnColision, string.Empty);
            minedEmitter.Emitter = groupEmitter;
            minedEmitter.CooldownTime = 1;
            minedEmitter.ActivationCheck.AddCost(MeterType.MiningSpeed, 10);
            agent.AddSystem(minedEmitter);
            agent.gameObjectType |= GameObjectType.Mineable | GameObjectType.CraftingStation;

            return agent;
        }
    }
}
