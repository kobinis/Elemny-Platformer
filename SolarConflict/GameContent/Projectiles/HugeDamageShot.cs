using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class HugeDamageShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add14";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "60";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(1000);
            profile.Mass = 0.1f;
           // profile.ImpactEmitterID = "FullExplosionFx1";
            profile.CollisionSpec = new CollisionSpec(20000, 3f);         
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
