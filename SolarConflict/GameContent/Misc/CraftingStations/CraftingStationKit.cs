using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems {
    class CraftingStationKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "Crafting", 100, "Crafting Station" , 1);
            kit.Profile.DescriptionText = "Enables crafting of engines, shields, generators...";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "CraftingStation");
      //      kit.Profile.CraftingStationType = Framework.CraftingStationType.CraftingStation;
            kit.Profile.SlotType |= SlotType.CraftingStation;            
            kit.Profile.EquippedTextureID = "Crafting";
            kit.Profile.EquippedTextureScale = 0.5f;
            kit.Profile.CraftingStationType = Framework.CraftingStationType.CraftingStation;
            //kit.Profile.us
            return kit;
        }
    }
}
