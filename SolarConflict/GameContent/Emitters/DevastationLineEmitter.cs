using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.NewContent.Projectiles;

namespace SolarConflict.NewContent.Emitters
{
    class DevastationLineEmitter
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(DevastationShot).Name;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
            emitter.PosAngleRange = 0;

            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadMin = -4000;
            emitter.PosRadRange = 8000;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Const;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 0;
            emitter.VelocityMagMin = 0;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 0.1f;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.MinNumberOfGameObjects = 3;
            emitter.RangeNumberOfGameObject = 4;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 60 * 5;

            emitter.SizeType = ParamEmitter.InitSizeType.Random;
            emitter.SizeBase = 150;
            emitter.SizeRange = 50;

            return emitter;
        }
    }
}
