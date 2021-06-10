using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SolarConflict.NewContent.Projectiles;

namespace SolarConflict.GameContent.Emitters
{
    class HyperSpaceJumpFx
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(FxSpark).Name;

            // Change emitted projectile Size
            emitter.SizeBase = 700;
            emitter.SizeRange = 20;
            emitter.SizeType = ParamEmitter.InitSizeType.Const;

            // Emitted projectile velocity
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
            emitter.VelocityAngleRange = 360;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 0.1f;
            emitter.VelocityMagRange = 1f;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = MathHelper.ToDegrees(0.1f);

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.MinNumberOfGameObjects = 20;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 40;
            emitter.LifetimeRange = 5;

            return emitter;
        }
    }
}

