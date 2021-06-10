using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.CraftingStations
{
    /// <summary>
    /// Used to craft advanced items from level 6 and above
    /// </summary>
    class AdvancedCraftingStation
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("AdvCrafting", 1000);//Size= 100
            var agent = ShipQuickStart.Make(data);
            agent.gameObjectType |= GameObjectType.CraftingStation;
            agent.gameObjectType &= ~GameObjectType.Ship;
            agent.CraftingStationType = CraftingStationType.AdvancedCraftingStation;
            agent.isControllable = false;
            agent.Inventory = null;
            agent.control.controlAi = null;
         //   agent.Light = Lights.LargeLight(Color.Blue);
            agent.Name = "Advanced Crafting Station";
            agent.InteractionSystem = new ActivitySwitcherSystem("CraftingActivity");

            //Basic gear
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);

            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter("AdvancedCraftingStationKit");
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
