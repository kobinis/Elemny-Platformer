using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Agents;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems {
    class MiningStationKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "MiningStation", 100, "Mining Station", 10);
            kit.Profile.DescriptionText = "Places Mining Station near you";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "MiningStation");
           // kit.Profile.CraftingStationType = Framework.CraftingStationType.ResourceMine | Framework.CraftingStationType.Mining;
            kit.Profile.SlotType |= SlotType.CraftingStation;
            return kit;
        }
    }
}
