using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class MineLine
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "ProximityMine";
            emitter.MinNumberOfGameObjects = 4;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
            emitter.PosRadMin = 0;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Range;
            emitter.VelocityMagMin = -100;
            emitter.VelocityMagRange = 200;
            emitter.RotationBase = 90;
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedBase = 10;
            emitter.RotationSpeedRange = 10;
            emitter.VelocityAngleBase = 0;
            return emitter;
        }
    }
}
