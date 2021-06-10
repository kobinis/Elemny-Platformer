using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class KineticMineBarrage1
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "KineticMine";
            emitter.MinNumberOfGameObjects = 9;
                        
            emitter.VelocityAngleRange = 160;            
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.RangeCenterd;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;            
            emitter.VelocityMagMin = 2;
            
            // No idea what a PosRad is, but it doesn't visibly affect the projectiles
            /**
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
            emitter.PosRadMin = 5;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.RangeCenterd;
            emitter.PosAngleRange = 160;/**/

            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;            

            return emitter;            
        }
    }
}
