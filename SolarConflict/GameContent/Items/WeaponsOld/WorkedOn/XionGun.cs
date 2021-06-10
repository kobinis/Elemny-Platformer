using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Projectiles.Shots;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;

namespace SolarConflict.GameContent.Items {
    class XionGun {
        public static Item Make() {
            WeaponData weaponData = MakeData();
            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }

        public static WeaponData MakeData() {
            //, "Particle cannon with outstanding damage, offset by low rate of fire, and significant kickback.\nCaptures tachyonic xions and slows them sufficiently to interact with baryonic matter, often catastrophically."
            WeaponData weaponData = new WeaponData("Xion Decelerator", 4, "SniperItem", null);
            weaponData.ShotEmitterID = typeof(XionShot).Name;
            weaponData.KickbackForce = 1.5f;
            weaponData.Cooldown = Utility.Frames(2.25f);
            weaponData.ShotSpeed = 40;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level);
            weaponData.ItemData.SellRatio = 0.5f;
            weaponData.SoundEffectEmitterID = "sound_shot1";

            return weaponData;
        }
    }
}