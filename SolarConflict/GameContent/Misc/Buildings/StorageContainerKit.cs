using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.ContentGeneration.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Misc.Buildings
{
    class StorageContainerKit
    {
        public static Item Make()
        {
            var kit = ShipConstructionKitGenerator.MakeItem("StorageContainer", "Container1", 2000, "Storage Container", 1);
            kit.Profile.DescriptionText = "Places a container that can store items.\n#hcolor{}To deploy place one of the first four slots in your inventory#dcolor{}";            
            kit.Profile.SlotType = SlotType.Consumable;
            return kit;
        }
    }
}
