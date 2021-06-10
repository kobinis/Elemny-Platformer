using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.Effects
{
    /// <summary>
    /// A splash of green blood
    /// </summary>
    class BigBloodSplashFx
    {
        public static ParamEmitter Make()
        {
            var emitter = BloodSplashFx1.Make();
            emitter.SizeBase = 50;
            emitter.SizeRange = 20;
            emitter.SizeType = ParamEmitter.InitSizeType.Random;
            return emitter;
        }
    }
}
