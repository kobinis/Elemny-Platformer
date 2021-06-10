using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// Slowing homing goo ball, lasts for 5 sec
    /// </summary>
    class GooBall
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "goo";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "10";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "600";
            profile.InitHitPointsID = "50";
            profile.Mass = 0.1f;
           // profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name; //remove
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.Force = 0.8f;
            profile.CollisionSpec.ForceType = ForceType.Mult;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.VelocityInertia = 1;
            profile.Mass = 1;
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.8f, 20);
            profile.Draw = new ProjectileDrawRotateWithTime(-0.1f, 0.1f, "goo", "goo");// new ProjectileDrawRotateWithTime();
            return profile;
        }

    }
}
