using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.CraftingStations
{
    class VanityCraftingStation
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("CraftingStation", 1000);//Size= 100
            var agent = ShipQuickStart.Make(data);
            agent.Name = "Vanity Crafting Station";
            agent.gameObjectType |= GameObjectType.CraftingStation;
            agent.gameObjectType &= ~GameObjectType.Ship;
            agent.CraftingStationType = CraftingStationType.Vanity;
            agent.isControllable = false;
            agent.Inventory = null;
            agent.control.controlAi = null;

            //Basic gear
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.InteractionSystem = new ActivitySwitcherSystem("CraftingActivity");

            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter("VanityCraftingStationKit");
            groupEmitter.AddEmitter("RemoveGameObjectEmitter");
            EmitterCallerSystem minedEmitter = new EmitterCallerSystem(ControlSignals.OnColision, string.Empty);
            minedEmitter.Emitter = groupEmitter;
            minedEmitter.CooldownTime = 5;
            minedEmitter.ActivationCheck.AddCost(MeterType.MiningSpeed, 1);
            agent.AddSystem(minedEmitter);


            return agent;
        }
    }
}
