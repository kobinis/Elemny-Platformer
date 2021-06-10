using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.Effects
{
    class SmallNebulaEmitter
    {
        public static IEmitter Make()
        {
            ParamEmitter emitter = new ParamEmitter(MakeClusterEmitter());
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            emitter.PosAngleRange = 360;
            //emitter.posa

            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadMin = 700;
            emitter.PosRadRange = 3000;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.MinNumberOfGameObjects = 15;

            emitter.SizeType = ParamEmitter.InitSizeType.Random; //add Parent //parentSizeTransformed
            emitter.SizeBase = 1000;
            emitter.SizeRange = 300f;

            return emitter;
        }

        public static IEmitter MakeClusterEmitter()
        {
            ParamEmitter emitter = new ParamEmitter(ContentBank.Inst.GetEmitter("NebulaFx1"));
            emitter.PosAngleType = ParamEmitter.EmitterPosAngle.Range;
            emitter.PosAngleRange = 360;

            emitter.PosRadType = ParamEmitter.EmitterPosRad.Random;
            emitter.PosRadMin = 10;
            emitter.PosRadRange = 1500;

            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            emitter.MinNumberOfGameObjects = 3;

            emitter.SizeType = ParamEmitter.InitSizeType.Random; //add Parent //parentSizeTransformed
            emitter.SizeBase = 500;
            emitter.SizeRange = 500f;

            return emitter;
        }
    }
}

