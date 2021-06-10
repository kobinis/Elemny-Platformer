using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NewContent.Emitters
{
    class VoidBeamFlash
    {
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(SmallVoidFlameFx).Name;
            emitter.MinNumberOfGameObjects = 2;
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Const;
            emitter.RotationSpeedRange = 1.0f;
            emitter.RotationSpeedBase = 2.0f;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 220;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
            emitter.PosRadMin = 4;
            emitter.PosRadRange = 3;
            emitter.SizeType = ParamEmitter.InitSizeType.Const;
            emitter.SizeBase = 30;
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Range;
            emitter.LifetimeMin = 6;
            emitter.LifetimeRange = 1;
            emitter.RefVelocityMult = 2.225f;
            
            return emitter;
        }
    }
}
