using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Projectiles.Shots;
using SolarConflict.GameContent.Utils;
using System;

namespace SolarConflict.GameContent.Items {
    /// <summary>Version of the Flak Gun in which the projectile explodes into a single AoE</summary>
    class FlakGun {
        public static Item Make() => WeaponQuickStart.Make(MakeData());

        public static WeaponData MakeData() {
            //, "Shoots explosive munitions on a timed fuse."
            WeaponData weaponData = new WeaponData("Flak Gun", 6, "FlakGunItem", "Shotgun");
            weaponData.ShotEmitterID = typeof(FlakShot1).Name;
            weaponData.KickbackForce = 0.15f;
            weaponData.Cooldown = Utility.Frames(0.33f);
            weaponData.ShotSpeed = 60;
            weaponData.EnergyCost = 50;
            weaponData.ItemData.BuyPrice = 1600;
            weaponData.ItemData.SellRatio = 0.5f;
            //Tweaks.ApplyTags(ref weaponData, "shot", "bomb");
            return weaponData;
        }        
    }
}
