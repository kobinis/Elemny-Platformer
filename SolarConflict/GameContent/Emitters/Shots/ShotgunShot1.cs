using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class ShotgunShot1
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = "Shot1";
            emitter.MinNumberOfGameObjects = 20;
            emitter.RangeNumberOfGameObject = 5;
            emitter.VelocityAngleRange = 45;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 10;
            emitter.VelocityMagRange = 5;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            return emitter;
        }
    }
}
