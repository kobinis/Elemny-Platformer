//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.NewContent.Emitters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolarConflict.GameContent.Items
//{
//    class Shotgun
//    {
//        public static Item Make()
//        {
//            WeaponData weaponData = new WeaponData("Shotgun");
//            weaponData.ItemData.IconID = "Shotgun";
//            weaponData.ItemData.EquippedTextureId = "turret1";
//            weaponData.ItemData.Level = 2;

//            weaponData.Range = 2500;
//            weaponData.ShotSpeed = 30;
//            weaponData.ShotLifetime = (int)(weaponData.Range / weaponData.ShotSpeed);
//            weaponData.Cooldown = 60;
//            weaponData.ActiveTime = 1;
//            float damage = 40;
//            weaponData.ShotEmitter = MakeShotCluster(damage);
//            weaponData.EnergyCost = 20 ;
//            //2 weaponData.IsTurreted = false;
//            weaponData.ItemData.BuyPrice = 200;
//            weaponData.KickbackForce = 0.5f;
//            weaponData.EffectEmitterID = "EmitterFxSmoke";
//            //TODO: speed mult = 0;
//            Item item = WeaponQuickStart.Make(weaponData);
//           // item.Profile.Level = 0;
//            return item;
//        }

//        public static ParamEmitter MakeShotCluster(float damage)
//        {
//            ParamEmitter emitter = new ParamEmitter();
//            emitter.MinNumberOfGameObjects = 20;
//            emitter.RangeNumberOfGameObject = 5;

//            var expectedNumProjectiles = (emitter.RangeNumberOfGameObject * 0.5f) + emitter.MinNumberOfGameObjects;
//            emitter.Emitter = MakeShot(damage / expectedNumProjectiles);

//            emitter.VelocityAngleRange = 90;
//            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
//            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
//            emitter.VelocityMagMin = 10;
//            emitter.VelocityMagRange = 5;
//            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
//            return emitter;
//        }

//        public static ProjectileProfile MakeShot(float damage)
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.ColorLogic = ColorUpdater.FadeOutSlow;
//            profile.InitColor = new InitColorConst(Color.White); //new InitColorConst(ScalingUtils.EffectColor(level));
//            profile.TextureID = "add10";
//            profile.CollisionWidth = profile.Sprite.Width - 10;
//            profile.InitSizeID = "25";
//            profile.UpdateSize = null;
//            profile.InitMaxLifetime = new InitFloatRandom(60, 30);
//            profile.Mass = 0.1f;
//            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name;

//            profile.CollisionSpec = new CollisionSpec(damage, ImpactType.Additive, 0.5f);
//            profile.IsDestroyedOnCollision = true;
//            profile.IsEffectedByForce = false;
//            return profile;
//        }        
//    }
//}
