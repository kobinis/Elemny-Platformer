using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NewContent.Emitters
{
    class EmitterGunFlash
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();//add constructor who gets id            
            emitter.EmitterID = typeof(SmalFlameFx).Name;
            emitter.MinNumberOfGameObjects = 4;
            emitter.RefVelocityMult = 0.6f;// 0.7f;
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 10;
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
            emitter.LifetimeMin = 3;
            return emitter;
        }
    }
}
