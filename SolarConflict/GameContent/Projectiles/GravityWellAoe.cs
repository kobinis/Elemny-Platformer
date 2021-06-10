using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class GravityWellAoe
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "add11";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatConst(15);
            projectileProfile.UpdateSize = new UpdateSizeGrow(15, 1, 1500);
            projectileProfile.InitMaxLifetime = new InitFloatConst(60*8);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(Color.DarkGray);
            projectileProfile.CollisionSpec = new CollisionSpec(0, -10);
            projectileProfile.VelocityInertia = 0.8f;
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            return projectileProfile;
        }
    }
}
