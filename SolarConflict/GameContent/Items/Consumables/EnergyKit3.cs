using SolarConflict.GameContent.Utils.QuickStart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Consumables
{
    class EnergyKit3
    {
        public static Item Make()
        {
            KitData data = new KitData("Small Energy Kit", "EnergyKit1", MeterType.Energy, 500, Framework.Utils.Utility.Frames(0.5F), ControlSignals.OnLowEnergy);
            data.ItemData.BuyPrice = 300;
            return KitQuickStart.Make(data);
        }
    }
}
