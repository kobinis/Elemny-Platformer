//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items {
//    /// <summary>Version of the Flak Gun in which the projectile explodes into a bunch of secondary projectiles. Not presently usable.</summary>
//    /// <remarks>The projectiles tend to penetrate targets' collision shapes before detonating, meaning all the flak gets instantaneously absorbed by said target. Gotta take measures.
//    /// Simplest fix would be to just increase the size of the flak shell's collision shape, but that's not terribly robust. Alternatively, could do custom emission logic, so the flak isn't all
//    /// spawned at the same point, or could check for penetration and backtrack the projectile until it's outside the collider.</remarks>
//    class FlakGunAlternative {
//        public static Item Make() => WeaponQuickStart.Make(MakeData());

//        public static WeaponData MakeData() {
//            WeaponData weaponData = new WeaponData("Flak Gun (Shrapnel)", "Shoots munitions that explode into shrapnel.", 6, "replace", "replace");
//            weaponData.ShotEmitterID = "FlakShot2";
//            weaponData.KickbackForce = 0.15f;
//            weaponData.Cooldown = Utility.Frames(0.33f);
//            weaponData.ShotSpeed = 60;
//            weaponData.ItemData.BuyPrice = 1600;
//            weaponData.ItemData.SellRatio = 0.5f;

//            return weaponData;
//        }
//    }
//}
