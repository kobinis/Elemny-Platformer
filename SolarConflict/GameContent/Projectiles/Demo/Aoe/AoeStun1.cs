using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class AoeStun1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "add3";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatConst(15);
            projectileProfile.UpdateSize = new UpdateSizeGrow(15);
            projectileProfile.InitMaxLifetime = new InitFloatConst(60);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(new Color(100,100,155));
            projectileProfile.CollisionSpec = new CollisionSpec(0, 0f, 60*2);
            projectileProfile.VelocityInertia = 0.8f; //??
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0); //??
            projectileProfile.Draw = new ProjectileDrawRotateWithTime(0.01f, -0.012f, "add3" ,"add3");
            return projectileProfile;
        }
    }
}
