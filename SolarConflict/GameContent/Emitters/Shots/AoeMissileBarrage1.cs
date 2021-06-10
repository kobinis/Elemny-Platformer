using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class AoeMissileBarrage1
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "MissileProj1";
            emitter.MinNumberOfGameObjects = 10;
            emitter.RangeNumberOfGameObject = 0;
            emitter.VelocityAngleRange = 270;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.RangeCenterd;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 20;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            return emitter;
        }
    }
}
