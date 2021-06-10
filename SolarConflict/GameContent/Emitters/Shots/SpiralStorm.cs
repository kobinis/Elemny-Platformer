using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.Shots
{
    class SpiralStorm
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "Shot1";
            emitter.MinNumberOfGameObjects = 20;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            emitter.PosAngleRange = 360;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.ParentSizeTransformed;
            emitter.PosRadMin = 10;
            emitter.PosRadRange = 1;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngleTransformed;
            emitter.VelocityAngleBase = 90;
            emitter._velocityAngleRange = 1;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 20;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            return emitter;
        }
    }
}
