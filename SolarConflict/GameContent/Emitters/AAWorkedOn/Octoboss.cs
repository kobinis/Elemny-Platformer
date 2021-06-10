using SolarConflict.Framework.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.AAWorkedOn
{
    class OctobossLegs
    {
        public static IEmitter Make()
        {
            var emitter = new HierarchyEmitter();
            emitter.AddEmitter("WormSegment");
            emitter.NumberOfObjects = 20;            

            ParamEmitter legs = new ParamEmitter(emitter);            
            legs.MinNumberOfGameObjects = 3;
            legs.PosAngleType = ParamEmitter.EmitterPosAngle.RangeCenterd;
            legs.PosAngleRange = 180;
            legs.PosRadType = ParamEmitter.EmitterPosRad.Const;
            legs.PosRadMin = 150;
            legs.SizeType = ParamEmitter.InitSizeType.Const;
            legs.SizeBase = 50;
            legs.RotationType = ParamEmitter.EmitterRotation.PosAngle;

            //var octoboss = new HierarchyEmitter();
            //octoboss.AddEmitter(legs);
            //octoboss.AddEmitter("OctobossHead");

            return legs;
        }
    }
}
