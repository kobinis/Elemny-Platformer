using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.NewContent.Emitters
{
    class EmitterGrenade
    {
        public static ParamEmitter Make()
        {

            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter("grenade"));


            emitter.MinNumberOfGameObjects = 1;
            emitter.RangeNumberOfGameObject = 1;
            emitter.ID = "EmitterGrenade";

            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityAngleRange = 360;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 0;
            emitter.VelocityMagRange = 1;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 10;

            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 200;

            return emitter;
        }
    }
}
