using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class MissileLauncher3
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Missile Launcher 3");
            //weaponData.ItemData.Description = "Lunches mines from your inventory";
            weaponData.ItemData.IconID = "RocketLauncher";
            weaponData.ItemData.SecounderyIconID = "lvl3";
            weaponData.ItemData.EquippedTextureId = "turret1";
            weaponData.ItemData.Level = 3;
            weaponData.ItemData.BuyPrice = (int)(ScalingUtils.ScaleCost(3) * 1.5f);
            weaponData.Cooldown = 30;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 45;
            weaponData.ActiveTime = 3;
            weaponData.ItemData.SlotType = SlotType.Weapon | SlotType.Turret;            
            weaponData.ItemData.Category = ItemCategory.AmmoWeapon;
            weaponData.ItemData.CraftingStationType = Framework.CraftingStationType.MissileAmmo;
            weaponData.AmmoType = ItemCategory.Missiles;
            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }
    }
}
