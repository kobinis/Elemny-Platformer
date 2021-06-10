using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.Effects
{
    class BigExplosion
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter("ExplosionParticleFx2"));

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
            emitter.VelocityAngleRange = 360;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 10f;
            emitter.VelocityMagRange = 5f;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 0.2f;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;


            emitter.MinNumberOfGameObjects = 3;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 120;
            emitter.LifetimeRange = 20;

            emitter.SizeType = ParamEmitter.InitSizeType.ParentSizeTransformed; //add Parent //parentSizeTransformed
            emitter.SizeBase = 0.8f;
            emitter.SizeRange = 1.2f;

            return emitter;
        }
    }
}
