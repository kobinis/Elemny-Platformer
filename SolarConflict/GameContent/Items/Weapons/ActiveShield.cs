using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class ActiveShield
    {
        public static Item Make()
        {
            //, "Stops most types of shots"
            WeaponData data = new WeaponData("Active Shield", 5, "ActiveShield");
            data.Description = "Blocks most types of shots for a limited time.";

            data.ItemData.BuyPrice = ScalingUtils.ScaleCost(data.ItemData.Level) * 2;
            data.ItemData.SlotType = SlotType.Utility;
            data.ItemData.Category = ItemCategory.Utility;
            data.ShotLifetime = 60 * 2;
            data.Cooldown = 60 * 10;
            data.ShotEmitterID = "Shield";
            data.EffectEmitterID = null;
            var item = WeaponQuickStart.Make(data);
            return item;
        }
    }
}
