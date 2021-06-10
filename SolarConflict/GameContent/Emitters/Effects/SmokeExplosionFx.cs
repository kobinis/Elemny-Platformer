using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.NewContent.Emitters
{
    class SmokeExplosionFx
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter("ProjFxSmoke1"));            
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Const;
            emitter.VelocityAngleRange = 360;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 0f;
            emitter.VelocityMagRange = 0f;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.RangeCenterd;
            emitter.RotationSpeedRange = MathHelper.ToDegrees(0.05f);

            emitter.RotationType = ParamEmitter.EmitterRotation.RangeCenterd;
            emitter.RotationRange = 360;


            emitter.MinNumberOfGameObjects = 2;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 160;
            emitter.LifetimeRange = 0;

            emitter.SizeType = ParamEmitter.InitSizeType.ParentSizeTransformed;
            emitter.SizeRange = 2f;

            return emitter;
        }
    }
}



