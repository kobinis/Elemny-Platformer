using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Emitters;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.NewContent.Projectiles;

namespace SolarConflict.GameContent.NewItems
{
    class TensorCannon
    {
        public static Item Make()
        {
            //, "Particle Hyper Detachment Device\nShoot it at a sun and see what happens"
            WeaponData data = new WeaponData("Tensor Cannon", 6, "SlotEnergyNetLauncher", "SlotEnergyNetLauncher");
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
            emitter.Emitter = MakeShot();
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
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

        public static ProjectileProfile MakeShot()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOut;
            profile.TextureID = "add15";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "70";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(60 * 5);
            profile.Mass = 0.5f;
            profile.CollisionSpec = new CollisionSpec(3, 0.5f);
            profile.InitHitPointsID = "500";
           // profile.HitPointZeroEmiiterID = typeof(HugeDamageShot).Name;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false; // Maybe change to true
            //profile.Draw = new ProjectileDrawRotateWithTime(0.01f, -0.015f, "add14", "add14");
            return profile;
        }
    }
}



//using System.Collections.Generic;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.Framework.Utils;
//using SolarConflict.NewContent.Projectiles;
//using SolarConflict.GameContent.Projectiles;
//using SolarConflict.GameContent.Utils;

//namespace SolarConflict.GameContent.Items
//{
//    /// <summary>Wide beam weapon with a windup time.</summary>
//    class TensorCannon
//    {
//        public static Item Make()
//        {

//            WeaponData weaponData = new WeaponData("Tensor Cannon", "Boundless destructive potential.", 0, "EnergyDrainWarhead", "BoomerangGun");
//            weaponData.ShotEmitterID = "BigLaserEmitter";
//            weaponData.KickbackForce = 0.1f;
//            weaponData.Cooldown = Utility.Frames(3f); ;
//            weaponData.ShotSpeed = 35;
//            weaponData.ItemData.BuyPrice = 3000;
//            weaponData.ItemData.SellRatio = 0.5f;

//            Item item = WeaponQuickStart.Make(weaponData);

//            var chargeTime = Utility.Frames(1.5f);   


//            ContentBank.Inst.AddContent(MakeFx1(chargeTime));

//            var firingSequence = new List<IProcess>();

//            // TODO: add self-targeted slowing effect during charge (or maybe just rotation-damping effect). Two possible incarnations: (1) actual brakes, i.e. a multiplier and cap for velocity,
//            // easily achieved with a self-collision with ForceType.Mult, or (2) systems disruption, diminishes the effect of thrust/rotation systems, but doesn't affect other forces/torques.
//            // Latter is more appropriate, but heftier.
//            firingSequence.Add(new Emit("sound_tensor_charge"));
//            firingSequence.Add(new Emit("TensorCannon_ChargeFx", chargeTime, 3));
//            // TODO: add sound emitter for "tensor_fire", then tweak the beam and sound duration until satisfied.
//            firingSequence.Add(new Emit("sound_tensor_fire"));
//            firingSequence.Add(new Emit(typeof(BigLaserEmitter).Name, Utility.Frames(0.67f)));
//            // TODO: visuals: maybe add a second emitter to send pulses along the beam

//            var system = new ProcessRunner(0, new ProcessChain(firingSequence));

//            system.CooldownTime = weaponData.Cooldown;
//            system.ActivationCheck.AddCost(MeterType.Energy, 1200f); // BUG: this doesn't work

//            item.MainSystem = system;

//            return item;
//        }


//        static ParamEmitter MakeFx1(int chargeTime) {
//            var result = new ParamEmitter();
//            result.ID = "TensorCannon_ChargeFx";

//            // TODO: maybe replace this with proper inward-radiating anime lines
//            result.EmitterId = typeof(SmalFlameFx).Name;
//            result.MinNumberOfGameObjects = 1;
//            result.RangeNumberOfGameObject = 1;

//            result.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
//            result.PosAngleBase = 0;
//            result.PosAngleRange = 360;

//            result.PosRadMin = 100;

//            result.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.RangeCenterd;
//            result.VelocityMagMin = -10f;            

//            return result;
//        }
//    }    
//}
