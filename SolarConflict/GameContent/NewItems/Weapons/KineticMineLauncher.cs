using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems
{
    class KineticMineLauncher
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Kinetic Mine Launcher");
            //weaponData.ItemData.Description = "Shoots 9 Kinetic mines that do damage acoording to speed";
            weaponData.ItemData.IconID = "KineticMineLauncher";
            weaponData.ItemData.EquippedTextureId = "item3";
            weaponData.ItemData.Level = 2;
            weaponData.ShotEmitterID = "KineticMineBarrage1";
            weaponData.ShotSpeed = 8; //5
            weaponData.ItemData.BuyPrice = 1000;
            weaponData.Cooldown = 60*4;
            weaponData.Range = 1600;
            
            //weaponData.IsTurreted = false;
            Item item = WeaponQuickStart.Make(weaponData);
            SystemHolder system = new SystemHolder(item.System, Vector2.Zero, 0);
            system.FixToCenter = true;
            item.Profile.Category = ItemCategory.Utility;
            item.System = system;            
            return item;
        }
    }
}
