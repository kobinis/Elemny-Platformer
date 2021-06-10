//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Projectiles.Shots;
//using SolarConflict.GameContent.Utils;
//using System;

//namespace SolarConflict.GameContent.Items {
//    /// <summary>TEMP class, because FlakGun is not presently amenable to scaling</summary>
//    class HeavyFlakGun {
//        public static Item Make() => WeaponQuickStart.Make(MakeData());

//        public static WeaponData MakeData() {
//            WeaponData weaponData = new WeaponData("Heavy Flak Gun", "Shoots explosive munitions on a timed fuse.", 8, "replace", "replace");            
//            weaponData.ShotEmitterID = typeof(HeavyFlakShot1).Name;
//            weaponData.KickbackForce = 0.15f;
//            weaponData.Cooldown = Utility.Frames(0.33f);
//            weaponData.ShotSpeed = 60;
//            weaponData.ItemData.BuyPrice = 1600;
//            weaponData.ItemData.SellRatio = 0.5f;

//            //Tweaks.ApplyTags(ref weaponData, "shot", "bomb");

//            return weaponData;
//        }        
//    }
//}
