using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Emitters.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Shots {
    class FlakExplosion1 {                
        public static ProjectileProfile Make() {            
            var profile = new ProjectileProfile();

            profile.VelocityInertia = 0f;
            profile.UpdateEmitter = ContentBank.Inst.GetEmitter(typeof(FlakExplosionFx1).Name); // visuals
            profile.InitSizeID = "325";
            profile.InitMaxLifetimeID = "1";
            profile.Mass = 0.1f;
            profile.CollisionSpec = new CollisionSpec(160f, 20f);

            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;

          //  profile.ApplyTags("aoe");

            return profile;
        }
    }
}
