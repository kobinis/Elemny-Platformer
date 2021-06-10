using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class AttackDronesBay
    {
        public static Item Make() //TODO: change to photon torpedo, needs energy
        {
            WeaponData weaponData = new WeaponData("Attack drones bay");
            weaponData.ItemData.IconID = "AttackDronesBayItem";
            weaponData.ItemData.EquippedTextureId = "experimental-ship-mod-3";
            weaponData.ItemData.Level = 2;
            weaponData.ActiveTime = 10;
            weaponData.MidCooldownTime = 3;
            weaponData.ItemData.SlotType = SlotType.Utility;

            weaponData.ShotEmitterID = "AttackDrone";
            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level);
            weaponData.Cooldown = 60*10;
            weaponData.ShotLifetime = 60 * 10;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 15;
            weaponData.ItemData.BuyPrice = 200;
            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }
    }
}
