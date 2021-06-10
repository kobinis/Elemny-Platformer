using SolarConflict.Framework.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.WorldObjects
{
    class Worm1
    {
        public static IEmitter Make()
        {
            var emitter = new HierarchyEmitter();
            emitter.PassParent = false;
            emitter.AddEmitter("WormSegment");
            emitter.NumberOfObjects = 15;
            emitter.NumberOfObjectsRange = 5;
            emitter.Size = 10;           
            return emitter;
        }
    }
}
