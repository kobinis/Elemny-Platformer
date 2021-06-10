using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.AAAInProgress
{
    class RotationLockItem
    {
        public static Item Make()
        {
            ItemData itemData = new ItemData("Rotation Lock", 2, "TauntRS");
            itemData.Category |= ItemCategory.Rotation;
            itemData.SlotType = SlotType.Rotation | SlotType.Utility;
            itemData.BuyPrice = 1000;
            Item item = ItemQuickStart.Make(itemData);
            item.System = new RotationFixer(0);
            item.Profile.Category |= ItemCategory.Final | ItemCategory.NonAI;
            return item;
        }
    }
}
