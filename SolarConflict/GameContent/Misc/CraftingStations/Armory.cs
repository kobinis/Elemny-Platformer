using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolarConflict.GameContent.Agents
{
    /// <summary>
    /// Used to craft weapons and ammo
    /// </summary>
    class Armory
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("Armory", 1000);//Size= 100
            var agent = ShipQuickStart.Make(data);
            agent.gameObjectType |= GameObjectType.CraftingStation;
            agent.gameObjectType &= ~GameObjectType.Ship;
            agent.CraftingStationType = CraftingStationType.Armory;
            agent.isControllable = false;
            agent.Inventory = null;
            agent.control.controlAi = null;
            agent.Name = "Armory";

            //Basic gear
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.InteractionSystem = new ActivitySwitcherSystem("CraftingActivity");

            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter("ArmoryKit");
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
