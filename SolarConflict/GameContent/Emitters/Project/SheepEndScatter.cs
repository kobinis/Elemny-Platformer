using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.NewContent.Emitters
{
    class SheepEndScatter
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter("KineticShot1"));
            emitter.ID = "SheepEndScatter";
            emitter.MinNumberOfGameObjects = 20;
            emitter.RangeNumberOfGameObject = 10;
            emitter.VelocityAngleRange = MathHelper.ToDegrees(360);
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 10;
            emitter.VelocityMagRange = 5;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;                 
            return emitter;
        }
    }
}
