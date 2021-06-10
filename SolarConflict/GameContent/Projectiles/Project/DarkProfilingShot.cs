using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles {
    /// <summary>Effects and knockback only, for profiling</summary>
    class DarkProfilingShot {
        public static ProjectileProfile Make() {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "Fusion";
            profile.CollisionWidth = profile.Sprite.Width + 20;
            profile.InitSizeID = "60";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(150);
            profile.Mass = 0.1f;

            profile.UpdateEmitterCooldownTime = 1;

            profile.CollisionSpec = new CollisionSpec(1f, 2f); // actually doing a bit of damage, for a test
            profile.CollisionSpec.ForceType = ForceType.FromCenter;
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;

            return profile;
        }
    }
}
