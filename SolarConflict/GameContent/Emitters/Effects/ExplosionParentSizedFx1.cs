using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.Effects
{
    class ExplosionParentSizedFx1
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(ExpParticleParentSizedFx1).Name;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 0.2f;
            emitter.VelocityMagRange = 1f;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = MathHelper.ToDegrees(0.03f);

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;


            emitter.MinNumberOfGameObjects = 3;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 60;
            emitter.LifetimeRange = 5;

            return emitter;
        }
    }
}
