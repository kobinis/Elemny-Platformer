using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.NewContent.Emitters
{
    class HugeExplosionEmitter
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


            emitter.MinNumberOfGameObjects = 20;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 60;
            emitter.LifetimeRange = 5;

            emitter.SizeType = ParamEmitter.InitSizeType.Const; //add Parent //parentSizeTransformed
            emitter.SizeBase = 8500;
            emitter.SizeRange = 100;            

            return emitter;
        }
    }
}
