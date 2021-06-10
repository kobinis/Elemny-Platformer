using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class NuclearExplosion
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "add1";
          //  projectileProfile.CollisionWidth = projectileProfile.Texture.Width - 2;
            projectileProfile.InitSize = new InitFloatConst(15);
            projectileProfile.UpdateSize = new UpdateSizeGrow(15,1.005f);
            projectileProfile.InitMaxLifetime = new InitFloatConst(90);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(Color.White);
            projectileProfile.CollisionSpec = new CollisionSpec(1, 15);
            projectileProfile.VelocityInertia = 1f;
            //projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            return projectileProfile;
        }
    }
}
