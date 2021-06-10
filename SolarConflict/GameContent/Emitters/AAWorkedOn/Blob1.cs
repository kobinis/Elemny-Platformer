using SolarConflict.Framework.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters.WorldObjects
{
    class Blob1
    {
        public static IEmitter Make() //TODO: not in a line
        {
            var emitter = new HierarchyEmitter();
            emitter.AddEmitter("BlobSegment");
            emitter.NumberOfObjects = 15;
            emitter.NumberOfObjectsRange = 10;
            return emitter;
        }
    }
}
