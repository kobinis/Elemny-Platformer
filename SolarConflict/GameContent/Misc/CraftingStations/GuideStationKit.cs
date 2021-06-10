using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Misc.CraftingStations
{
    class GuideStationKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem(null, "MaterialAnalyzer", 2000, "Material Analyzer", 1);
            kit.Profile.DescriptionText = "Places a #color{255,255,0}Material Analyzer#dcolor{} near you.";
            kit.System = new BasicEmitterCallerSystem(ControlSignals.None, "GuideStation");            
            return kit;
        }
    }
}
