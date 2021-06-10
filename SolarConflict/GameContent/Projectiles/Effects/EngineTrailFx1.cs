using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Effects {
    class EngineTrailFx1 {
        public static ProjectileProfile Make() {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.Effects;

          //  profile.Lights = new ILight[] { Lights.ContactLight() };

            profile.InitMaxLifetime = new InitFloatConst(1);//texture from param
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
