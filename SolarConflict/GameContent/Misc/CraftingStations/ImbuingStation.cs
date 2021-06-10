using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Misc.CraftingStations
{
    class ImbuingStation
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("ImbuingStation", 1000);//Size= 100
            var agent = ShipQuickStart.Make(data);
            //  agent.gameObjectType |= GameObjectType.CraftingStation;
            agent.gameObjectType &= ~GameObjectType.Ship;
            // agent.CraftingStationType = CraftingStationType.AdvancedArmory;
            agent.isControllable = false;
            agent.Inventory = null;
            agent.control.controlAi = null;
       //     agent.Light = Lights.LargeLight(Color.Purple);
            agent.Name = "Imbuing Station";

            //Basic gear
            //agent.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
            //agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            //agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
            agent.InteractionSystem = new ActivitySwitcherSystem("ImbueActivity");

            GroupEmitter groupEmitter = new GroupEmitter();
            groupEmitter.AddEmitter("ImbuingStationKit");
            groupEmitter.AddEmitter("RemoveGameObjectEmitter");
            EmitterCallerSystem minedEmitter = new EmitterCallerSystem(ControlSignals.OnColision, string.Empty);
            minedEmitter.Emitter = groupEmitter;
            minedEmitter.CooldownTime = 5;
            minedEmitter.ActivationCheck.AddCost(MeterType.MiningSpeed, 1);
            agent.AddSystem(minedEmitter);
            agent.gameObjectType |= GameObjectType.Mineable | GameObjectType.CraftingStation;
            agent.CraftingStationType |= Framework.CraftingStationType.ImbuingStation ;


            return agent;
        }
    }
}
