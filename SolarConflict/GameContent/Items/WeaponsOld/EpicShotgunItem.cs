using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Emitters;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;

namespace SolarConflict.GameContent.Items
{
    class EpicShotgunItem
    {
        public static Item Make()
        {

            WeaponData weaponData = new WeaponData("Kinetic Shotgun", 8, "ShotgunIcon", "Shotgun");         
            weaponData.ItemData.Category = ItemCategory.EnergyConsumingWeapon | ItemCategory.Shotgun | ItemCategory.Gun;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(8);
            weaponData.Cooldown = 60;
            weaponData.ShotEmitterID = "ShotgunShot3";
            weaponData.ShotSpeed = 10;
            weaponData.SoundEffectEmitterID = "sound_shotgun";
            weaponData.EffectEmitterID = "GunFlashFx";
            weaponData.EnergyCost = 25;
            weaponData.KickbackForce = 5;
            return WeaponQuickStart.Make(weaponData);
        }
    }
}
