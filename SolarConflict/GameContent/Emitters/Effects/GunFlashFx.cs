using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    /// <summary>
    /// Forward flesh of fire effect (To be used when gun fires)
    /// </summary>
    class GunFlashFx
    {        
        public static ParamEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.EmitterID = typeof(SmalFlameFx).Name;
            emitter.MinNumberOfGameObjects = 3;            
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 10;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;
            emitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
            emitter.PosRadMin = 10;
            emitter.PosRadRange = 12;
            emitter.SizeType = ParamEmitter.InitSizeType.Const;
            emitter.SizeBase = 25;
            emitter.LifetimeType = ParamEmitter.EmitterLifetime.Range;
            emitter.LifetimeMin = 7;
            emitter.LifetimeRange = 4;
            return emitter;
        }
    }
}
