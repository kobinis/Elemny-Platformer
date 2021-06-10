using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Consumables
{
    class RepairKit6
    {
        public static Item Make()
        {
            KitData data = new KitData("Large Repair Kit", "RepairKit1", MeterType.Hitpoints, 1400, Utility.Frames(10), ControlSignals.OnLowHitpoints, true);            
            data.SecoundImpactType = ImpactType.Additive;
            data.SecoundMeterType = MeterType.Shield;
            data.SecoundValue = 2000;
            data.ItemData.BuyPrice = 1900;
            data.ItemData.Level = 6;
            data.ItemData.SecounderyIconID = "lvl6";
            return KitQuickStart.Make(data);
        }
    }
}
