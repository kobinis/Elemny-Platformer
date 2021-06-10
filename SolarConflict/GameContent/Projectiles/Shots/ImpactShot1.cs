using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots {
    /// <warning>
    /// While the knockback effect on most ships is roughly as satisfying as intended, the knockback effect on asteroids is comical.
    /// TODO: maybe allow colliders to take a callback for force, or to otherwise adjust their impact force depending on the type of the other collider.
    /// </warning>
    class ImpactShot1 {
        public static ProjectileProfile Make() {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add5";
            profile.CollisionWidth = profile.Sprite.Height - 5; // why -5?
            profile.InitSizeID = "25";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "200";
            profile.Mass = 40f;            
            profile.CollisionSpec = new CollisionSpec(90, 600f);
            // TODO: if we're cool with enabling torque, maybe add some rotational damping on hit for about half a second, to make that torque limiting            
            
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = true;
            return profile;
        }
    }
}
