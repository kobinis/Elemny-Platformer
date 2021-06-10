using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles;

namespace SolarConflict.NewContent.Emitters
{
    class EmitterImpactFxVoid1
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter(typeof(SmallVoidHitParticleFx).Name));            

            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadMin = 0;
            emitter.PosRadRange = 3;

            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = MathHelper.ToDegrees(MathHelper.TwoPi);

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;//??? what uis normal change
            emitter.VelocityAngleRange = MathHelper.ToDegrees(MathHelper.TwoPi);

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 0.2f;
            emitter.VelocityMagRange = 0.2f;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = MathHelper.ToDegrees(0.15f);

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = MathHelper.TwoPi;

            emitter.MinNumberOfGameObjects = 2;
            emitter.RangeNumberOfGameObject = 2;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 10;
            emitter.LifetimeRange = 5;

            emitter.SizeType = ParamEmitter.InitSizeType.Random;
            emitter.SizeBase = 30;
            emitter.SizeRange = 20;

            emitter.RefVelocityMult = 0;

            return emitter;

        }
    }
}
