using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters {
    class BigLaserEmitter {
        public static ParamEmitter Make() {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(BigLaserBit).Name;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
            emitter.PosRadMin = 100;
            emitter.PosRadRange = 1900;

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
            emitter.VelocityMagMin = 0;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Const;
            emitter.RotationSpeedRange = 0;

            emitter.MinNumberOfGameObjects = 150;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 1;

            return emitter;
        }
    }
}
