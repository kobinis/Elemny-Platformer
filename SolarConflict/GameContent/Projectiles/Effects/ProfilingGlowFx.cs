using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles {
    /// <summary>An eternal flame.</summary>
    class ProfilingGlowFx {
        public static ProjectileProfile Make() {
            ProjectileProfile profile = new ProjectileProfile();

          //  profile.Lights = new ILight[] { Lights.MediumLight(Color.Orange) };
            
            profile.TextureID = "add5";
            profile.CollisionWidth = 32;
            profile.InitMaxLifetime = new InitFloatConst(float.MaxValue);
            profile.InitSizeID = "20";
            
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.Additive;
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
