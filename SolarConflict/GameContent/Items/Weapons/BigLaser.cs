using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.NewContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class BigLaser
    {
        public static Item Make()
        {
            //, "Boundless destructive potential"
            WeaponData data = new WeaponData("B.F.L", 7, "BigLaserItem", null);
            data.ItemData.BuyPrice = ScalingUtils.ScaleCost(data.ItemData.Level);
            data.ItemData.SellRatio = 0.5f;
            data.ItemData.IsRatiendOnDeath = true;
            data.ItemData.Size = SizeType.Medium;
            data.WarmupTime = 20;
            data.WarmupEmitter = MakeWarmupEmitter();
            data.ShotEmitter = MakeLaserEmitter();
            data.Cooldown = 90;
            data.ActiveTime = 30;
            data.ActivationEmitterID = "sound_tensor_charge";
            data.EndOfWarmpupEmitterID = "sound_tensor_fire";
            data.SoundEffectEmitterID = null;
            data.EffectEmitterID = "GunFlashFx";
            data.ItemData.SlotType = SlotType.Weapon;
            data.ItemData.Size = SizeType.Large; //Maybe make it large
            var item = WeaponQuickStart.Make(data);
            return item;
        }

        public static IEmitter MakeWarmupEmitter()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "FxSpark";
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
            emitter.PosRadMin = 0;
            emitter.PosRadRange = 1900;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 0;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 3;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.MinNumberOfGameObjects = 150;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 5;
            return emitter;
        }

        public static IEmitter MakeLaserEmitter()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.Emitter = MakeLaserBit();
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
            emitter.PosRadMin = 50;
            emitter.PosRadRange = 1900;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 0;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Const;
            emitter.RotationSpeedRange = 0;

            emitter.MinNumberOfGameObjects = 150;
            //emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            //emitter.RotationRange = 360;
            emitter.SizeType = ParamEmitter.InitSizeType.Const;
            emitter.SizeRange = 0.8f;
            emitter.SizeBase = 150;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 1;
            return emitter;
        }

        public static ProjectileProfile MakeLaserBit()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(200, 50, 50));
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add10";
            profile.CollisionWidth = profile.Sprite.Width - 10; // why -10?
            profile.InitSizeID = "100";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(1);
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(LaserHitFx).Name; // TODO: change effect
            profile.CollisionSpec = new CollisionSpec(3f, 0.1f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            // profile.ApplyTags("energy", "large"/*, "bright"*/);
            return profile;
        }
    }
}
