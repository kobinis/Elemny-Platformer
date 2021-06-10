using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class grenade
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "Grenade";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "20";
            profile.UpdateSize = null;
            profile.RotationLogic = new UpdateRotationForward();
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.05f, 7f);

            profile.InitMaxLifetime = new InitFloatConst(300);
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = "EmitterSelfDestruct"; //Explosion When Hit
            profile.TimeOutEmitterID = "EmitterSelfDestruct"; //Explosion When time ended
            profile.CollisionSpec = new CollisionSpec(500, 0.5f);
            profile.IsDestroyedOnCollision = true;

            profile.IsEffectedByForce = false;
            return profile;


        }
    }
}
