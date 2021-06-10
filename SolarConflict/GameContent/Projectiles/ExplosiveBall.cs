using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class ExplosiveBall
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.CollisionType = CollisionType.CollideAll;
            //profile.UpdateColor = ProjectileProfile.ColorUpdate.FadeOutSlow;
            profile.TextureID = "attention"; //??
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitSizeID = "50";                        
            profile.Mass = 0.2f;
            //profile.impactUpdateList.Add(new ImpactChangeParent());
            // Add set parent
            //
            //profile.ImpactEmitterId = "EmitterImpactFx1";
            profile.HitPointZeroEmiiterID = "SelfDestruct";
            profile.InitHitPointsID = "30";
            profile.VelocityInertia = 0.99f;
            profile.CollisionSpec = new CollisionSpec(0, 0.5f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.IsTurnedByForce = true;
            return profile;
        }
    }
}
