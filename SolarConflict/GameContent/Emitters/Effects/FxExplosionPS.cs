using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Emitters.Effects
{
    class FxExplosionPS
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "SmokePS";

            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            emitter.PosAngleRange = 360;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.ParentSizeTransformed;
            emitter.PosRadRange = 0.5f;


            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;
            emitter.VelocityAngleRange = 360;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 0.1f;
            emitter.VelocityMagRange = 1f;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = MathHelper.ToDegrees(0.1f);

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;


            emitter.MinNumberOfGameObjects = 5;
            emitter.SizeType = ParamEmitter.InitSizeType.ParentSizeTransformed;
            emitter.SizeRange = 0.3f;


            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 60;
            emitter.LifetimeRange = 5;

            return emitter;
        }
    }
}
