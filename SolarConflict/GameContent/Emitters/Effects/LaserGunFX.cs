using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Emitters.Effects
{
    class LaserGunFX
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(SmalFlameFx).Name;
            emitter.MinNumberOfGameObjects = 2;
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 10;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
            emitter.PosRadMin = 10;
            emitter.PosRadRange = 12;
            emitter.SizeType = ParamEmitter.InitSizeType.Const;
            emitter.SizeBase = 10;
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Range;
            emitter.LifetimeMin = 7;
            emitter.LifetimeRange = 0;
            return emitter;
        }
    }
}
