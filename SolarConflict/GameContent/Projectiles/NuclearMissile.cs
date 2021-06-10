using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.NewContent.Emitters;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class NuclearMissile
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.DrawType = DrawType.Alpha;
            projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;
            projectileProfile.TextureID = "item2";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 10;
            projectileProfile.InitSizeID = "15";
            projectileProfile.UpdateSize = null;
            projectileProfile.RotationLogic = new UpdateRotationForward();
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.05f, 7f);
            //profile.Draw = new ProjectileDrawRotateWithTime(0.1f, -0.12f);
            projectileProfile.InitMaxLifetime = new InitFloatConst(180);
            projectileProfile.Mass = 0.1f;
            projectileProfile.UpdateEmitterID =  typeof(EmitterFxSmoke).Name;
            projectileProfile.UpdateEmitterCooldownTime = 2;            
            projectileProfile.ImpactEmitterID = "FireExplosionFx";
            projectileProfile.CollisionSpec = new CollisionSpec(100, 0.5f);
            projectileProfile.IsDestroyedOnCollision = true;
            projectileProfile.IsEffectedByForce = false;
            return projectileProfile;
        }
    }
}
