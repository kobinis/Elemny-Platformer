using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SolarConflict.Framework;

namespace SolarConflict.GameContent.Agents
{
    /// <summary>
    /// Crafting Station is used to craft items up to level 4 (engines, rotation engines, utils + advanced crafting station) 
    /// </summary>
    class CraftingStation
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("Crafting", 1000);//Size= 100
            var agent = ShipQuickStart.Make(data);
            agent.gameObjectType |= GameObjectType.CraftingStation;
            agent.gameObjectType &= ~GameObjectType.Ship;
            agent.CraftingStationType = CraftingStationType.CraftingStation;

            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter("CraftingStationKit");
            groupEmitter.AddEmitter("RemoveGameObjectEmitter");
            EmitterCallerSystem minedEmitter = new EmitterCallerSystem(ControlSignals.OnColision, string.Empty);
            minedEmitter.Emitter = groupEmitter;
            minedEmitter.CooldownTime = 5;
            minedEmitter.ActivationCheck.AddCost(MeterType.MiningSpeed, 1);            

            agent.AddSystem(minedEmitter);

            //agent.AddSystem()

            agent.Inventory = null;
            agent.control.controlAi = null;
            agent.Name = "Crafting Station";

            ////Basic gear
            //agent.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
            //agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            //agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);

            agent.InteractionSystem = new ActivitySwitcherSystem("CraftingActivity");
            agent.isControllable = false;
            agent.gameObjectType |= GameObjectType.NonRotating;
            return agent;
        }
    }
}
