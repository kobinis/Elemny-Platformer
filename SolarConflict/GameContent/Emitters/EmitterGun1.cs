using SolarConflict.GameContent;
using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NewContent.Emitters
{
    class EmitterGun1 {
        public static GroupEmitter Make() {
            GroupEmitter emitter = new GroupEmitter();//add constructor who gets id
            emitter.AddEmitter(typeof(Shot1).Name);
            emitter.AddEmitter(typeof(EmitterGunFlash).Name); //shot fire FX
            emitter.AddEmitter(new SoundEmitter("shot5", 0.3f)); //change
            return emitter;
        }

    }
}
