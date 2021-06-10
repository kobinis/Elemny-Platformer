using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class RepairKit1
    {
        public static Item Make()
        {
            KitData data = new KitData("Small Repair Kit", "RepairKit1", MeterType.Hitpoints, 1000, Utility.Frames(30), ControlSignals.None, true);
            data.ItemData.BuyPrice = 100;
            data.ItemData.Level = 1;
            return KitQuickStart.Make(data);
        }
    }
}
