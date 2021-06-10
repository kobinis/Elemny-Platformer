using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class SunPushAura
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.None;
            projectileProfile.TextureID = null;            
            projectileProfile.InitSize = new InitFloatConst(15);            
            projectileProfile.IsDestroyedOnCollision = false;            
            projectileProfile.CollisionSpec = new CollisionSpec(0.5f, 3000f);
            projectileProfile.IsEffectedByForce = false;
            projectileProfile.IsDestroyedOnCollision = false;
            //projectileProfile.CollusionSpec.ForceType = ForceType.
            projectileProfile.VelocityInertia = 0.8f; //??
            //projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);

            return projectileProfile;
        }
    }
}
