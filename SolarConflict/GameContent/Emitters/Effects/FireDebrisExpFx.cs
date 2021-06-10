using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Emitters.Effects
{
    class FireDebrisExpFx
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "FireDebris";            
            emitter.MinNumberOfGameObjects = 3;
            emitter.RangeNumberOfGameObject = 3;

            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = 360;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
            emitter.PosRadMin = 20;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;
            //emitter.VelocityAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 10f;
            emitter.VelocityMagRange = 10f;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 0.2f;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 20;
            emitter.LifetimeRange = 30;

            return emitter;
        }
    }
}
