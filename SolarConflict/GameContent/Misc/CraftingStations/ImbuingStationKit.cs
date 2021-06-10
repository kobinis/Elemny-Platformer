using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Misc.CraftingStations
{
    class ImbuingStationKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "ImbuingStation", 2000, "Imbuing Station", 1);
            kit.Profile.DescriptionText = "Places a #color{255,255,0}Imbuing Station#dcolor{} near you.";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "ImbuingStation");
            kit.Profile.SlotType = SlotType.CraftingStation;
            kit.Profile.CraftingStationType |= Framework.CraftingStationType.ImbuingStation | Framework.CraftingStationType.CraftingStation;
            kit.Profile.Category |= ItemCategory.Final;
            return kit;
        }
    }
}
