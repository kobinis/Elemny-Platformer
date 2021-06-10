using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class FireShot //Now: add traill
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add2";
            profile.CollisionWidth = profile.Sprite.Width + 20;
            profile.InitSizeID = "60";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.Mass = 0.1f;
            profile.UpdateEmitterCooldownTime = 3;
            profile.ImpactEmitterID = typeof(DamageAoe).Name;
            profile.CollisionSpec = new CollisionSpec(50, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;

          //  profile.ApplyTags(Color.OrangeRed.ToVector3(), "energy", "small");

            return profile;
        }
    }
}
