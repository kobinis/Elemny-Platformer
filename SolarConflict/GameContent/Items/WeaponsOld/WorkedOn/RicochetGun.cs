using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;

namespace SolarConflict.GameContent.Items {
    class RicochetGun {
        public static Item Make() => WeaponQuickStart.Make(MakeData());            
        

        public static WeaponData MakeData() {
            //, "Shoots highly elastic death."
            WeaponData weaponData = new WeaponData("Ricochet Gun", 3, "RicochetGunItem",null);            
            weaponData.ShotEmitterID = "RicochetShot1";
            weaponData.KickbackForce = 0.15f;
            weaponData.Cooldown = 10;
            weaponData.ShotSpeed = 70;
            weaponData.ItemData.Level = 2;
            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level) * weaponData.Cooldown;
            weaponData.ItemData.BuyPrice = 2000;
            weaponData.ItemData.SellRatio = 0.5f;
            //weaponData.ItemData.Description = ""; // TODO: once you know what best/worst-case DPS is, do a find/replace here. How to define best-case DPS is nontrivial; best option is
            // probably to make it the max damage dealt if each shot bounces off some unrelated target the max number of times before impacting the primary target once.
            // An alternate formulation would be total damage dealt to all targets if each shot bounces the max number of times and then impacts again, but that'd be really
            // misleadingly high.

            return weaponData;
        }
    }
}
