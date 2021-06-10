using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters {
    class MineArc {
        public static ParamEmitter Make() {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(ProximityMine).Name;
            emitter.MinNumberOfGameObjects = 9;

            emitter.VelocityAngleRange = 160;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.RangeCenterd;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 2;            

            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;

            return emitter;
        }
    }
}
