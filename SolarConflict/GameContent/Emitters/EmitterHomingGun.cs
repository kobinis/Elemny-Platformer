/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.NewContent.Emitters
{
    class EmitterHomingGun
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetGameObjectFactory("Shot3"));
            emitter.Id = "EmitterHomingGun";
            emitter.MinNumberOfGameObjects = 20;
            emitter.RangeNumberOfGameObject = 5;
            
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 10;
            emitter.VelocityMagRange = 5;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            return emitter;
        }
    }
}*/
