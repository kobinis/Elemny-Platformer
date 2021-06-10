using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;
using SolarConflict.GameContent;

namespace SolarConflict.NewContent.Projectiles
{
    class DevastationShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeInOut;
            //profile.InitColor = ;
            profile.TextureID = "fireball1";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "10";
            profile.UpdateSize = new UpdateSizeGrow(0, 1.005f);
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = "SmokeExplosionFx";
            profile.UpdateSize = new UpdateSizeGrow(1, 1f);
            //profile.ImpactEmitterId = "FireExplosionFx";
            profile.CollisionSpec = new CollisionSpec(80, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;

            //profile.ApplyTags("explosion", "medium");

            return profile;
        }
    }
}
