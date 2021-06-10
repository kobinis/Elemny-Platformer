using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems {
    class VanityCraftingStationKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "CraftingStation", 100, "Vanity Crafting Station",4);
            kit.Profile.DescriptionText = "Places Vanity Crafting Station";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "VanityCraftingStation");
         //   kit.Profile.CraftingStationType = Framework.CraftingStationType.Vanity;
            kit.Profile.SlotType |= SlotType.CraftingStation;
            return kit;
        }
    }
}
