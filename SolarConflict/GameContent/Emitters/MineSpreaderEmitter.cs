using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class MineSpreaderEmitter {
        public static ParamEmitter Make() {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(MineLine).Name;
            emitter.MinNumberOfGameObjects = 4;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
            emitter.PosRadMin = 0;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Range;
            emitter.VelocityMagMin = 50;
            emitter.VelocityMagRange = 250;
            emitter.RotationBase = 90;

            emitter.VelocityAngleBase = 0;
            return emitter;
        }        


    }
}
