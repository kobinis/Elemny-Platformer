using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles
{
    class MiniMissile
    {
        public static IEmitter Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();            
            projectileProfile.AggroRange = 2000;
            projectileProfile.DrawType = DrawType.Alpha;
            //projectileProfile.InitColor = new InitColorConst(colors[i]);
           // projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;
            projectileProfile.TextureID = "item2";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 10;
            projectileProfile.InitSizeID = "15";
            projectileProfile.UpdateSize = null;
            //projectileProfile.UpdateRotation = new UpdateRotationForward();
            //projectileProfile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.05f, 7f);
            projectileProfile.MovementLogic = new MoveForward(0.6f, 7);
            //profile.Draw = new ProjectileDrawRotateWithTime(0.1f, -0.12f);
            projectileProfile.InitMaxLifetime = new InitFloatConst(180);
            projectileProfile.Mass = 0.1f;
            projectileProfile.UpdateEmitterID = typeof(EmitterFxSmoke).Name;
            projectileProfile.UpdateEmitterCooldownTime = 2;
            projectileProfile.RotationLogic = new UpdateRotationHoming();
            projectileProfile.ImpactEmitterID = "FireFx";
            projectileProfile.TimeOutEmitterID = "EmitterDebris1";
            projectileProfile.CollisionSpec = new CollisionSpec(45, 0.5f);
            projectileProfile.CollisionSpec.IsDamaging = true;
            projectileProfile.IsDestroyedOnCollision = true;
            projectileProfile.IsEffectedByForce = false;
            projectileProfile.CollisionType = CollisionType.Collide1;
            return projectileProfile;
        }
    }
}
