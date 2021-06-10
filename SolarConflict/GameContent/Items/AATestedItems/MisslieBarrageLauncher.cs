//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.AATestedItems
//{
//    class MisslieBarrageLauncher
//    {
//        public static Item Make() //TODO: change to photon torpedo, needs energy
//        {
//            WeaponData weaponData = new WeaponData("Missila Baragge");
//            weaponData.ItemData.IconID = "TractorBeam";
//            weaponData.ItemData.EquippedTextureId = "turret1";
//            weaponData.ItemData.Level = 4;
//            weaponData.ActiveTime = 1;
//            weaponData.SoundEffectEmitterID = "sound_tensor_charge";


//            weaponData.ShotEmitterID = "AoeMissileBarrage1";
//            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level);
//            weaponData.Cooldown = 60;
//            //weaponData.IsTurreted = false;
//            weaponData.ShotSpeed = 50;
//            weaponData.ItemData.BuyPrice = 200;
//            Item item = WeaponQuickStart.Make(weaponData);
//            return item;
//        }
//    }
//}
