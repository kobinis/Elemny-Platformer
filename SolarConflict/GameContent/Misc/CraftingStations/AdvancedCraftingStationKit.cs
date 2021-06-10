using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems
{
    class AdvancedCraftingStationKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "AdvCrafting", 2000, "Advanced Crafting Station",5);
            kit.Profile.DescriptionText = "Enables crafting of advanced items";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "AdvancedCraftingStation");
            //   kit.Profile.CraftingStationType = Framework.CraftingStationType.AdvancedCraftingStation;
            kit.Profile.EquippedTextureID = "AdvCrafting";
            kit.Profile.SlotType |= SlotType.CraftingStation;
            kit.Profile.CraftingStationType = CraftingStationType.CraftingStation | CraftingStationType.AdvancedCraftingStation;
            return kit;
        }
    }
}
