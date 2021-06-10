using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Consumables
{
    class RepairKit3
    {
        public static Item Make()
        {
            KitData data = new KitData("Repair Kit", "RepairKit1", MeterType.Hitpoints, 1000, Utility.Frames(8), ControlSignals.OnLowHitpoints, true);
            data.SecoundImpactType = ImpactType.Additive;
            data.SecoundMeterType = MeterType.Shield;
            data.SecoundValue = 1000;
            data.ItemData.BuyPrice = 800;
            data.ItemData.Level = 3;
            data.ItemData.SecounderyIconID = "lvl3";
            return KitQuickStart.Make(data);
        }
    }
}
