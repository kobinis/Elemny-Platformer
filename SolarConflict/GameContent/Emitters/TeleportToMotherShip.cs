using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class TeleportToMotherShip
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(TeleportProjectile).Name; //typeof(TeleportWithEffect).Name;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.DistanceFromMotherShip;
            emitter.PosRadRange = 1;
            emitter.PosRadMin = -100;
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.AngleToMotherShip;
            return emitter;
        }
    }
}
