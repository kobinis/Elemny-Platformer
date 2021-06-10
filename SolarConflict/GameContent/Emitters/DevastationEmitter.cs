using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.NewContent.Projectiles;

namespace SolarConflict.NewContent.Emitters
{
    class DevastationEmitter
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(DevastationShot).Name;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            emitter.PosAngleRange = 360;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
            emitter.PosRadMin = 10;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 10;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 1;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.MinNumberOfGameObjects = 140;
            emitter.RangeNumberOfGameObject = 5;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 60 * 5;

            return emitter;
        }
    }
}
