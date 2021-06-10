using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class BlackHole
    {
        public static ProjectileProfile Make()
        {
            float sizeMultiplier = 6f;

            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add14"; //add14
            profile.CollisionWidth = profile.Sprite.Width * sizeMultiplier;
            profile.InitSizeID = "10";
            profile.UpdateSize = new UpdateSizeGrow(1, 1.015f, 550 * sizeMultiplier);
            profile.InitMaxLifetimeID = "600"; //1/60 sec
            profile.Mass = 0.1f;
            //profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name;
            //profile.InitHitPointsId = "1000";
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            //profile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.8f, 15); 
            //profile.UpdateRotation = new UpdateRotationForward();
            profile.CollisionSpec = new CollisionSpec(1.5f, -120f);
            profile.CollisionSpec.ForceType = ForceType.Gravity;
            profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;
            profile.VelocityInertia = 0.85f;
            //profile.Draw = new ProjectileDrawRotateWithTime(0.001f, -0.0012f, "add14");            

            // TODO: damaging inner effect

            return profile;
        }
        
    }
}
