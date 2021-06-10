using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NewContent.Emitters
{
    class FxEmitterCloak
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();//add constructor who gets id
            emitter.ID = "FxEmitterCloak";
            emitter.EmitterID = "ProjShipwreck1";
            emitter.MinNumberOfGameObjects = 5;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
            emitter.VelocityAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 1;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 15;
            return emitter;
        }
    }
}
