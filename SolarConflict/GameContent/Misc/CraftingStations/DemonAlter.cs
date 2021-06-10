using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.CraftingStations
{
    class DemonAlter
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("demonalter", 10000);//Size= 100
            var agent = ShipQuickStart.Make(data);
            agent.gameObjectType |= GameObjectType.CraftingStation;
            agent.gameObjectType &= ~GameObjectType.Ship;
            agent.CraftingStationType = CraftingStationType.DemonAlter;
            agent.isControllable = false;
            agent.Inventory = null;
            agent.control.controlAi = null;
            agent.Name = "Vile Alter";
            agent.gameObjectType |= GameObjectType.CraftingStation;
            ActivitySwitcherSystem switcher = new ActivitySwitcherSystem("CraftingActivity");
            switcher.InteractionText = "Craft";
            agent.InteractionSystem = switcher;

          ///  agent.Light = Lights.HugeLight(Color.Red);

            //Basic gear
            //agent.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
           // agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);
           // agent.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);

            return agent;
        }
    }
}
