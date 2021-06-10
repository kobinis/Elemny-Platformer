//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Utils;

//namespace SolarConflict.GameContent.Items {
//    class ImpactGun {
//        public static Item Make() {
//            WeaponData weaponData = MakeData();
//            Item item = WeaponQuickStart.Make(weaponData);
//            return item;
//        }

//        public static WeaponData MakeData() {
//            WeaponData weaponData = new WeaponData("Impact Cannon", "Shoots high-momentum impactors.", 0, "replace", "replace");
//            weaponData.ShotEmitterID = "ImpactShot1";
//            weaponData.KickbackForce = 3f;
//            weaponData.Cooldown = Utility.Frames(1.25f);
//            weaponData.ShotSpeed = 40;
//            weaponData.ItemData.BuyPrice = 1200;
//            weaponData.ItemData.SellRatio = 0.5f;

//            return weaponData;
//        }
//    }
//}