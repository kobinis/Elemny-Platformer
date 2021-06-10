using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Emitters;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.NewItems
{
    class StarDestroyerItem
    {
        public static Item Make()
        {
            //, "Particle Hyper Detachment Device\nShoot it at a sun and see what happens"
            WeaponData data = new WeaponData("Star Destroyer", 6, "LaserItem", null);
            //data.IsTurreted = true;
            data.ItemData.BuyPrice = 200000;
            data.ItemData.SellRatio = 0.5f;
            data.Range = 2000;
            data.ItemData.IsRatiendOnDeath = true;
            data.ItemData.Size = SizeType.Medium;
            data.WarmupTime = 40;
            data.WarmupEmitter = MakeWarmupEmitter();
            data.ShotEmitter = MakeLaserEmitter();
            data.Cooldown = 60 * 3;
            data.ActivationEmitterID = "sound_tensor_charge";
            data.SoundEffectEmitterID = "sound_tensor_fire";
            data.EffectEmitterID = "GunFlashFx";
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
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = 20;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 0;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 3;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.MinNumberOfGameObjects = 150;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 10;
            return emitter;
        }

        public static IEmitter MakeLaserEmitter()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(StarDestroyerShot).Name;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = 20;
            emitter.PosRadMin = 0;
            emitter.PosRadRange = 1900;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 0;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Const;
            emitter.RotationSpeedRange = 0;

            emitter.MinNumberOfGameObjects = 150;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;
            emitter.SizeType = ParamEmitter.InitSizeType.ParentSizeTransformed;
            emitter.SizeRange = 0.8f;
            emitter.SizeBase = 50;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 25;
            return emitter;
        }
    }
}
