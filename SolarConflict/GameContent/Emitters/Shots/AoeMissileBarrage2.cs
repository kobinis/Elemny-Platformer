using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class AoeMissileBarrage2
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "MissileProj1";
            emitter.MinNumberOfGameObjects = 23;
            emitter.RangeNumberOfGameObject = 0;
            emitter.PosAngleRange = 360;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.ParentSize;            
            emitter.VelocityAngleRange = 360;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 30;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            emitter.LifetimeMin = 60 * 15;
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            return emitter;
        }
    }
}
