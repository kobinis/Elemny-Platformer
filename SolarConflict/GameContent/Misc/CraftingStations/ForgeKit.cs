using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems {
    class ForgeKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "Forge", 100, "Forge",3);
            kit.Profile.DescriptionText = "Places forge near you";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "Forge");
            kit.Profile.SlotType |= SlotType.CraftingStation;
       //     kit.Profile.CraftingStationType |= Framework.CraftingStationType.Forge;
            return kit;
        }
    }
}
