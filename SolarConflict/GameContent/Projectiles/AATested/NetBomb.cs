using SolarConflict.GameContent.Emitters;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class NetBomb
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;
           // profile.UpdateColor = new UpdateColorSwitch(Color.White, Color.Red); //TODO: maybe change to countdown animation
            profile.TextureID = "attention"; //TODO: maybe change to countdown animation
            profile.CollisionWidth = profile.Sprite.Width - 5; //adjusts colider size to display size
            profile.InitSizeID = "20";
            profile.InitMaxLifetime = new InitFloatConst(60 * 7); //7 sec delay
            profile.Mass = 0.5f;
            // profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name;
            profile.CollisionType = CollisionType.Collide1;
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = true;
            profile.AggroRange = 30;
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 1f, 10);
            profile.VelocityInertia = 1;
            //profile.UpdateRotation = new UpdateRotationForward();
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.CollisionSpec.IsDamaging = true;
            profile.TimeOutEmitterID = typeof(SlowingNet).Name; //change explosion
           // profile.HitPointZeroEmiiterId = typeof(SlowingNet).Name;
            profile.ImpactEmitterID = typeof(SlowingNet).Name;
          //  profile.InitHitPointsId = "200";
            return profile;
        }
    }
}
