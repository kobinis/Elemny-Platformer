using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Emitters.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Shots {
    class FlakExplosion2 {

        public static ParamEmitter Make() {
            var emitter = new ParamEmitter();
            emitter.EmitterID = typeof(FlakExplosion2Shrapnel).Name;

            emitter.RefVelocityMult = 0f;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
            emitter.VelocityAngleRange = 360;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 3f;
            emitter.VelocityMagRange = 25f;            

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = MathHelper.ToDegrees(0.1f);

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.MinNumberOfGameObjects = 200;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 15;
            emitter.LifetimeRange = 5;            

            return emitter;
        }        
    }
}