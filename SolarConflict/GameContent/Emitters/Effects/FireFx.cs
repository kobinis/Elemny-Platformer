using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;

namespace SolarConflict.GameContent.Emitters
{
    class FireFx
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(FireSparkFx).Name;
            emitter.RefVelocityMult = 0.1f;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadMin = 1;
            emitter.PosRadRange = 5;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 0.1f;
            emitter.VelocityMagRange = 0.5f;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityAngleRange = 360;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 0.15f; //maybe more;
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 30;
            emitter.MinNumberOfGameObjects = 1;
            emitter.RangeNumberOfGameObject = 1;
            //mitter
            return emitter;
        }
    }
}
