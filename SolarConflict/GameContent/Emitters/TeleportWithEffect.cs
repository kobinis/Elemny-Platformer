using SolarConflict.Framework.GameObjects.Emitters;
using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    class TeleportWithEffect
    {
        public static GroupEmitter Make()
        {
            GroupEmitter emitter = new GroupEmitter();
            emitter.AddEmitter(typeof(TeleportAncestor).Name);
            emitter.AddEmitter(typeof(TeleportFx).Name);
            emitter.AddEmitter(typeof(FullScreenColorFX).Name); //TODO: change to screen effect
            return emitter;
        }
    }
}
