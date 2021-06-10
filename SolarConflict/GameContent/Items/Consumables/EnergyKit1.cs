using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class EnergyKit1
    {
        public static Item Make()
        {
            KitData data = new KitData("Small Energy Kit", "EnergyKit1", MeterType.Energy, 2000, Utility.Frames(0.5F), ControlSignals.OnLowEnergy);
            data.ItemData.Level = 2;
            data.ItemData.BuyPrice = 100;
            data.ItemData.Size = SizeType.Small;
            return KitQuickStart.Make(data);
        }
    }
}
