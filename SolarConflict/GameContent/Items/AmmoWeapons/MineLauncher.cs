using SolarConflict.Framework.Agents.Systems.EmitterCallers;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems.Mines
{
    class MineLauncher
    {
        public static Item Make() //TODO: change to photon torpedo, needs energy
        {
            WeaponData weaponData = new WeaponData("Mine Launcher");
            //weaponData.ItemData.Description = "Lunches mines from your inventory";
            weaponData.ItemData.IconID = "MineLauncher";
            weaponData.ItemData.EquippedTextureId = "turret1";
            weaponData.ItemData.Level = 2;
            weaponData.Cooldown = 60;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 15;
          //  weaponData.ItemData.SlotType = SlotType.Weapon;
            weaponData.ItemData.Category = ItemCategory.AmmoWeapon;
            weaponData.AmmoType = ItemCategory.Mines;
            weaponData.ItemData.BuyPrice = 1000;
            weaponData.Range = 1000;

            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }
    }
}
