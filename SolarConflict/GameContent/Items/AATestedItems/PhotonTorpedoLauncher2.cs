//using SolarConflict.GameContent.Projectiles;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.AATestedItems
//{
//    class PhotonTorpedoLauncher2
//    {
//        public static Item Make() //TODO: change to photon torpedo, needs energy
//        {
//            WeaponData weaponData = new WeaponData("Photon Torpedo Launcher 222");
//            weaponData.ItemData.Description = "Lunches Photon Torpedoes";
//            weaponData.ItemData.IconID = "replace";
//            weaponData.ItemData.EquippedTextureId = "turret1";
//            weaponData.ItemData.Level = 4;
//            weaponData.ActiveTime = 10;
//            weaponData.MidCooldownTime = 3;

//            ParamEmitter paramEmitter = new ParamEmitter();
//            paramEmitter.Emitter = PhotonTorpedoLauncher1.MakeMissile(ProjectileTargetType.None, 80, AoeDamage1.Make());
//            paramEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.RangeCenterd;
//            paramEmitter.RotationRange = 180;
//            paramEmitter.VelocityMagMin = 3;
//            paramEmitter.MinNumberOfGameObjects = 2;

//            weaponData.ShotEmitter = paramEmitter;
//            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level);
//            weaponData.Cooldown = 60;
//            weaponData.IsTurreted = false;
//            weaponData.ShotSpeed = 15;
//            weaponData.ItemData.BuyPrice = 200;
//            Item item = WeaponQuickStart.Make(weaponData);
//            item.Profile.SlotType = SlotType.Utility | SlotType.Consumable;
//            return item;
//        }
//    }
//}
