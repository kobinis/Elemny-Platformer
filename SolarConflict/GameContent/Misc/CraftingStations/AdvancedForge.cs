using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.CraftingStations
{
    class AdvancedForge
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("Forge", 1000);//Size= 100
            var agent = ShipQuickStart.Make(data);
            agent.gameObjectType |= GameObjectType.CraftingStation;
            agent.gameObjectType &= ~GameObjectType.Ship;
            agent.CraftingStationType = CraftingStationType.AdvancedForge;
            agent.isControllable = false;
            agent.Inventory = null;
            agent.control.controlAi = null;
            agent.Light = Lights.LargeLight(Color.Turquoise);
            agent.Name = "Advanced Forge";
            agent.InteractionSystem = new ActivitySwitcherSystem("CraftingActivity");

            //Basic gear
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);

            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter("AdvancedForgeKit");
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
