using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;

namespace SolarConflict.GameContent.Emitters
{
    class SlowingLine
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(NetNode).Name;
            emitter.MinNumberOfGameObjects = 15;                        
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
            emitter.PosRadMin = 4;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Range;
            emitter.VelocityMagMin = -200;
            emitter.VelocityMagRange = 400;

            emitter.VelocityAngleBase = 90; ;
/*            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Range;
            emitter.LifetimeMin = 10;
            emitter.LifetimeRange = 20;*/
            return emitter;
        }
    }
}
