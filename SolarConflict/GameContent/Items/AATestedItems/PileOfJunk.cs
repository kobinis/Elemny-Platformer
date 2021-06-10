using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class PileOfJunk
    {
        public static Item Make()
        {
            ItemData data = new ItemData("Pile Of Junk", 11, "pileofjunk");            
            data.SlotType = SlotType.Generator | SlotType.Shield | SlotType.Utility | SlotType.MainEngine;
            data.BuyPrice = 100;
            data.SellRatio = 1;
            Item item = ItemQuickStart.Make(data);
            SystemGroup sg = new SystemGroup();
            MeterGenerator mg = new MeterGenerator();
            sg.AddSystem(mg);
            item.System = sg;
            item.Profile.DescriptionText = "You can sell it for cash";
            return item;
        }
    }
}
