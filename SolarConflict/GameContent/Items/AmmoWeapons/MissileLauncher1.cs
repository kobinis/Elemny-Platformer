using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems.Missiles
{
    class MissileLauncher1
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Missile Launcher");
            // weaponData.ItemData.Description = "Lunches mines from your inventory";
            weaponData.ItemData.IconID = "RocketLauncher";
            weaponData.ItemData.EquippedTextureId = "turret1";
            weaponData.ItemData.Level = 1;
            weaponData.ItemData.BuyPrice = (int)(ScalingUtils.ScaleCost(1) * 1.5f);
            weaponData.Cooldown = 60;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 15;
            weaponData.ItemData.SlotType = SlotType.Weapon | SlotType.Turret;
            weaponData.ItemData.Category = ItemCategory.AmmoWeapon;
            weaponData.ItemData.CraftingStationType = Framework.CraftingStationType.MissileAmmo;
            weaponData.AmmoType = ItemCategory.Missiles;
            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }
    }
}
