using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles.Temp
{
    public class BoomerangBomb
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "spikeball";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "15";
            profile.UpdateSize = null;
            profile.RotationLogic = new UpdateRotationForward();
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.05f, 7f);
            //profile.Draw = new ProjectileDrawRotateWithTime(0.1f, -0.12f);
            profile.InitMaxLifetime = new InitFloatConst(180);
            profile.Mass = 0.1f;
            profile.UpdateEmitter = ContentBank.Inst.GetEmitter("EmitterFxSmoke");
            profile.UpdateEmitterCooldownTime = 2;
            profile.ImpactEmitterID = "FireExplosionFx";
            profile.CollisionSpec = new CollisionSpec(100, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
