using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots {
    /// TEMP
    class HeavyFlakShot1 {
        public static ProjectileProfile Make() {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "shot2";
            profile.CollisionWidth = profile.Sprite.Height - 5;
            profile.InitSizeID = "10";
            profile.InitMaxLifetimeID = (Utility.Frames(0.67f)).ToString();
            // ^ Note compensation for speed tweaks to keep range constant
            profile.Mass = 0.1f;
            profile.CollisionSpec = new CollisionSpec(3f, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = true;

            profile.ImpactEmitterID = typeof(HeavyFlakExplosion1).Name;
            profile.TimeOutEmitterID = typeof(HeavyFlakExplosion1).Name;

           // profile.ApplyTags("shot", "bomb");

            return profile;
        }
    }
}