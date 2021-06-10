using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems
{
    class AdvancedArmoryKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "Armory", 2000, "Advanced Armory",5);
            kit.Profile.DescriptionText = "Enables the crafting of advanced weapons";            
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "AdvancedArmory");
          //  kit.Profile.CraftingStationType = Framework.CraftingStationType.AdvancedArmory;
            kit.Profile.SlotType |= SlotType.CraftingStation;
            kit.Profile.CraftingStationType = Framework.CraftingStationType.Armory | Framework.CraftingStationType.AdvancedArmory;
            return kit;
        }
    }
}
