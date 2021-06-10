using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameContent.Projectiles;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class SpaceMine1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();            
            profile.CollisionType = CollisionType.CollideAll;
            profile.InitColor = new InitColorFaction();
            profile.DrawType = DrawType.Alpha;
            //profile.UpdateColor = ProjectileProfile.ColorUpdate.FadeOut;
            profile.TextureID = "item3";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "20";
            profile.UpdateSize = null; // new UpdateSizeGrow(1.1f);
            profile.InitMaxLifetime = new InitFloatConst(1000);  // 1/60 of a second
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(DamageAoe).Name;
            profile.TimeOutEmitterID = typeof(DamageAoe).Name;
            profile.CollisionSpec = new CollisionSpec(5, 0.5f);
            profile.IsDestroyedOnCollision = true; //projectile is terminated on impact
            profile.IsEffectedByForce = false;
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.005f, 3);
            profile.VelocityInertia = 0.992f;
            
            return profile;
        }
    }
}