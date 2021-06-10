using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Reflection;
using SolarConflict.GameContent.Projectiles;

namespace SolarConflict.NewContent.Emitters
{
    class EmitterSelfDestruct
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter(typeof(DamageAoe).Name));
            emitter.ID = MethodBase.GetCurrentMethod().DeclaringType.Name;

            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadMin = 0;
            emitter.PosRadRange = 6;

            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = MathHelper.ToDegrees(MathHelper.TwoPi);

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;
            emitter.VelocityAngleRange = MathHelper.ToDegrees(MathHelper.TwoPi);

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 10f;
            emitter.VelocityMagRange = 3f;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 0.15f;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = MathHelper.ToDegrees(MathHelper.TwoPi);

            emitter.MinNumberOfGameObjects = 10;
            emitter.RangeNumberOfGameObject = 2;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 90;
            emitter.LifetimeRange = 5;

            return emitter;
        }
    }
}
