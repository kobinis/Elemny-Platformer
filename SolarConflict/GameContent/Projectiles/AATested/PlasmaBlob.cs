using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    /// <summary>
    /// Heals 
    /// </summary>
    class PlasmaBlob
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.AggroRange = 4500;
            profile.DrawType = DrawType.Additive;
            profile.TextureID = "add7";
            profile.Draw = profile.Draw = new ProjectileDrawRotateWithTime(-0.1f, 0.1f, "add7", "add7");
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitHitPointsID = "100";
            profile.InitSizeID = "20";
            profile.InitMaxLifetime = new InitFloatConst(60 * 60 * 2);
            profile.Mass = 0.5f;
            profile.CollisionType = CollisionType.Collide1;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;            
            var movement = new MoveToTarget(ProjectileTargetType.Enemy, 0.2f, 6, true);
            movement.SecondaryTarget = ProjectileTargetType.Parent; //or maybe ancestor
            profile.MovementLogic = movement;
            profile.VelocityInertia = 1f;
            profile.RotationLogic = new UpdateRotationForward();
            profile.CollisionSpec = new CollisionSpec(0, 1.5f);
            profile.CollisionSpec.IsDamaging = true;
            //Change draw
            return profile;
        }
    }
}
