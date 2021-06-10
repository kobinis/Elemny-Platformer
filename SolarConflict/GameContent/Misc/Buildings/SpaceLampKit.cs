using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Misc.Buildings {
    class SpaceLampKit {
        public static Item Make() {
            var result = ShipConstructionKitGenerator.MakeItem("SpaceLamp", "light1", 1000, "Space Lamp", 2);
            result.Profile.DescriptionText = "Deploys a lamp. In... in space.";
            result.Profile.SlotType = SlotType.Consumable;
            return result;
        }
    }
}
