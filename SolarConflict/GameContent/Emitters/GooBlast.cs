using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class GooBlast
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(GooBall).Name;
            emitter.MinNumberOfGameObjects = 21;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Random;
            emitter.PosAngleRange = 360;

            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadMin = 2;
            emitter.PosRadRange = 20;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 20;
            emitter.VelocityMagRange = 10;
                        
            return emitter;

        }
    }
}
