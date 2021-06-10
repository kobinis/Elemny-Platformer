//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolarConflict.GameContent.Items.AAAInProgress
//{
//    class Lightsaber
//    {
//        public static Item Make()
//        {
//            //, "Emits a cone of fire."
//            WeaponData data = new WeaponData("Lightsaber", 4, "Lightsaber");
//            data.ShotEmitter = MakeLaserEmitter();
//            data.ItemData.SecounderyIconID = "lvl" + data.ItemData.Level.ToString();
//            data.EnergyCost = 10;
//            data.ShotSpeed = 25;
//            data.SoundEffectEmitterID = null;
//            data.Cooldown = 1;
//            data.EffectEmitterID = null;
//            data.ItemData.BuyPrice = (int)(ScalingUtils.ScaleCost(data.ItemData.Level) * 3f);
//            Item item = WeaponQuickStart.Make(data);
//            //item.Profile.Level = 0;
//            item.Profile.Category = ItemCategory.Final;
//            return item;
//        }

//        public static IEmitter MakeLaserEmitter()
//        {
//            ParamEmitter emitter = new ParamEmitter();
//            emitter.Emitter = MakeShot();
//            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
//            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
//            emitter.PosRadMin = 50;
//            emitter.PosRadRange = 1900;

//            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

//            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
//            emitter.VelocityMagMin = 0;

//            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Const;
//            emitter.RotationSpeedRange = 0;

//            emitter.MinNumberOfGameObjects = 150;
//            //emitter.RotationType = ParamEmitter.EmitterRotation.Random;
//            //emitter.RotationRange = 360;
//            emitter.SizeType = ParamEmitter.InitSizeType.ParentSizeTransformed;
//            emitter.SizeRange = 0.8f;
//            emitter.SizeBase = 150;

//            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
//            emitter.LifetimeMin = 1;
//            return emitter;
//        }


//        public static ProjectileProfile MakeShot()
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.InitColor = new InitColorConst(new Color(200, 50, 50));
//            profile.ColorLogic = ColorUpdater.FadeOutSlow;
//            profile.TextureID = "add10";
//            profile.CollisionWidth = profile.Sprite.Width - 10;
//            profile.InitSizeID = "10";
//            profile.UpdateSize = null;
//            profile.InitMaxLifetime = new InitFloatConst(1);
//            profile.Mass = 0.1f;
//            profile.ImpactEmitterID = typeof(LaserHitFx).Name;
//            profile.UpdateEmitterID = "SmokePS";
//            profile.CollisionSpec = new CollisionSpec(0.4f, 0f);
//            profile.CollisionSpec.AddEntry(MeterType.MiningLevel, 2, ImpactType.Max);
//            profile.IsDestroyedOnCollision = true;
//            profile.IsEffectedByForce = false;
//            return profile;
//        }
//    }
//}
