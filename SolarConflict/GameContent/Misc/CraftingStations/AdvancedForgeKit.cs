using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems
{
    class AdvancedForgeKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "Forge", 2000, "Advanced Forge",10);
            kit.Profile.DescriptionText = "Places a Forge near you. Use it to advanced basic items.";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "AdvancedForge");
         //   kit.Profile.CraftingStationType = Framework.CraftingStationType.AdvancedForge;
            kit.Profile.SlotType |= SlotType.CraftingStation;
            return kit;
        }
    }
}
