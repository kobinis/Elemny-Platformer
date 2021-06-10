using SolarConflict.Framework.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.AAWorkedOn
{
    class BigWorm
    {
        public static IEmitter Make()
        {
            var emitter = new HierarchyEmitter();
            emitter.PassParent = false;
            emitter.AddEmitter("BigWormSegment");
            emitter.NumberOfObjects = 15;
            emitter.NumberOfObjectsRange = 10;
            emitter.Size = 2;
            return emitter;
        }
    }
}
