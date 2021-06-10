using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots
{
    /// <summary>
    /// A shoot that emits other shots in a random diraction
    /// </summary>
    class SparkShot1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            //profile.ColorLogic = ColorUpdater;
            profile.TextureID = "add7";
            profile.Draw = new ProjectileDrawRotateWithTime(-0.11f, 0.1f, "add7", "add7");
            profile.InitSizeID = "30";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name; //TODO: change
            profile.TimeOutEmitterID = typeof(EmitterImpactFx1).Name; //TODO: change
            profile.CollisionSpec = new CollisionSpec(40, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;

            ParamEmitter shotEmitter = new ParamEmitter();
            shotEmitter.EmitterID = "FireShot1"; //"Shot1";// "RicochetShot1";
            shotEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            shotEmitter.PosAngleRange = 360;
            shotEmitter.PosRadMin = 10;
            shotEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;
            shotEmitter.VelocityMagMin = 10;
            shotEmitter.RotationType = ParamEmitter.EmitterRotation.PosAngle;
            shotEmitter.SizeType = ParamEmitter.InitSizeType.Const;
            shotEmitter.SizeBase = 10;
            profile.UpdateEmitter = shotEmitter;
            return profile;
        }
    }
}
