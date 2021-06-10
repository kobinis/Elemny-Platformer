using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class EnergyNetLauncher
    {
        public static Item Make() //TODO: maybe needs ammo,
        {
            int level = 6;
            WeaponData data = new WeaponData("Energy Net Launcher");
            data.ItemData.IconID = "EnergyNetLauncher";
            data.ShotEmitterID = "NetBomb";
            data.ItemData.Level = level;
            data.ItemData.BuyPrice = 2 * ScalingUtils.ScaleCost(level);
            data.EnergyCost = 100;
            data.CooldownSec = 60;
            data.ItemData.SlotType = SlotType.Utility;
            data.ItemData.Category = ItemCategory.None; //Player only
            data.ShotSpeed = 30;            
            var item = WeaponQuickStart.Make(data);
            item.System = new TurretSystemHolder(item.System, Vector2.Zero, null);
            return item;
                                   
        }
    }
}
