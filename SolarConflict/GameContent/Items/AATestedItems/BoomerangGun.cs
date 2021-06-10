using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;

namespace SolarConflict.GameContent.Items
{
    class BoomerangGun
    {
        public static Item Make()
        {
            WeaponData weaponData = MakeData();
            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }

        public static WeaponData MakeData()
        {
            //"Shoots space boomerangs."
            WeaponData weaponData = new WeaponData("Boomerang Gun", 4, "EnergyDrainWarhead", "BoomerangGun");
            //weaponData.ShotEmitterID = "BoomerangShot";
            weaponData.Range = 1500;
            weaponData.KickbackForce = 0.1f;
            weaponData.Cooldown = 30;
            weaponData.ShotSpeed = 65;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level);
            weaponData.AmmoType = ItemCategory.Boomerang;
            weaponData.ItemData.CraftingStationType = Framework.CraftingStationType.BoomerangAmmo;
            return weaponData;
        }
    }
}
