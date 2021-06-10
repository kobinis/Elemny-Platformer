using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Projectiles;

namespace SolarConflict.NewContent.Emitters
{
    class EmitterMineCircle
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(SpaceMine1).Name;            
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            emitter.PosAngleRange = 360;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.ParentSize;            

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 6;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 10;

            emitter.MinNumberOfGameObjects = 5;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
            emitter.LifetimeMin = 60*20;
            emitter.LifetimeRange = 40;

            return emitter;
        }
    }
}
