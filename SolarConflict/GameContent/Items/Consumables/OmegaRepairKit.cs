using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.Consumables
{
    class OmegaRepairKit
    {
        public static Item Make()
        {
            KitData data = new KitData("Omega Repair Kit", "RepairKit1", MeterType.Hitpoints, 1400, Utility.Frames(5), ControlSignals.None, true);
            data.SecoundImpactType = ImpactType.Additive;
            data.SecoundMeterType = MeterType.Shield;
            data.SecoundValue = 8500;
            data.ItemData.BuyPrice = 1900;
            data.ItemData.Level = 11;
            data.ItemData.SecounderyIconID = "lvl6";           
            return KitQuickStart.Make(data);
        }
    }
}
