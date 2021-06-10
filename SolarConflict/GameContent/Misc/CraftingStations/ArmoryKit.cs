using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems
{
    class ArmoryKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "Armory", 100, "Armory" , 1);
            kit.Profile.DescriptionText = "Enables advanced weapons crafting.";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "Armory");
          //  kit.Profile.CraftingStationType = Framework.CraftingStationType.Armory;
            kit.Profile.SlotType |= SlotType.CraftingStation;
            kit.Profile.CraftingStationType = Framework.CraftingStationType.Armory;
            return kit;
        }
    }
}
