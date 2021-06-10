using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Emitters
{
    public class SunExplosion
    {
        public static GroupEmitter Make()
        {
            GroupEmitter emitter = new GroupEmitter();
            emitter.AddEmitter("NovaRingExplosion"); 
            emitter.AddEmitter("HugeExplosionEmitter"); 

            emitter.AddEmitter(new SoundEmitter("exp1", 0.3f)); //change
            return emitter;
        }
    }
}
