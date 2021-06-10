using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class GooBlastUtil
    {
        public static Item Make()
        {
            //, "Shots homing balls of goo that slow the enemy"
            WeaponData data = new WeaponData("Goo Glober", 3, "GooBlastUtil");
            data.ShotLifetime = 60 * 6;
            data.Cooldown = 60 * 15;
            data.ShotEmitterID = "GooBlast";
            data.EffectEmitterID = null;
            data.ItemData.BuyPrice = ScalingUtils.ScaleCost(data.ItemData.Level);
            var item = WeaponQuickStart.Make(data);            
            item.Profile.SlotType = SlotType.Utility;
            return item;
        }
    }
}
