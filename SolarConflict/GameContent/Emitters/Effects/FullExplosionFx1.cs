using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NewContent.Emitters
{
    class FullExplosionFx1
    {
        public static GroupEmitter Make()
        {
            GroupEmitter emitter = new GroupEmitter();
            emitter.RefVelocityMult = 0.1f;
            emitter.AddEmitter("ExplosionParentSizedFx1");// "FireExplosionFx"); 
            emitter.AddEmitter("SmokeExplosionFx");
            emitter.AddEmitter("ShockwaveFx");
            emitter.AddEmitter("EmitterDebris1");
            emitter.AddEmitter("ProjShipwreck1");
            emitter.AddEmitter(new SoundEmitter("exp2", 1)); //change 
            emitter.AddEmitter("FireDebrisExpFx");
            //emitter.AddEmitter(new )
            return emitter;
        }
    }
}
