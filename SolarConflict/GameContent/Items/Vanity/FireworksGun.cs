using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Vanity
{
    class FireworksGun
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Fireball");
            weaponData.ItemData.IconID = "add15";
            weaponData.ItemData.EquippedTextureId = "HouseGun";            
            weaponData.ItemData.Level = 5;
            weaponData.Cooldown = 15;
            weaponData.ActiveTime = 1;

            weaponData.ShotEmitterID = "FireballShot";// "Fireworks";// "Fireworks";
            //2 weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 30;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level)* 3;
            weaponData.KickbackForce = 0.1f;
            weaponData.EffectEmitterID = "EmitterFxSmoke";
            weaponData.SoundEffectEmitterID = null;
            //TODO: speed mult = 0;
            Item item = WeaponQuickStart.Make(weaponData);
          //  item.Profile.EquippedTextureScale = 1;
            return item;
        }
    }
}
