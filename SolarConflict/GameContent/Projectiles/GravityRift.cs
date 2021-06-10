using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles {
    class GravityRift {
        public static ProjectileProfile Make() {
            var profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add14"; // TEMP
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "10";
            profile.InitMaxLifetimeID = Utility.Frames(4f).ToString();
            
            
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            
            profile.CollisionSpec = new CollisionSpec(1f, -0.1f);
            profile.CollisionSpec.ForceType = ForceType.FromCenter;
            profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;

            profile.VelocityInertia = 0f;            

            return profile;
        }
    }
}