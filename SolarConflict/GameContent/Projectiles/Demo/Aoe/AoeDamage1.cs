using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class AoeDamage1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "bigshockwave";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatConst(15);
            projectileProfile.UpdateSize = new UpdateSizeGrow(15);
            projectileProfile.InitMaxLifetime = new InitFloatConst(25);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(new Color(255,200,150));
            projectileProfile.CollisionSpec = new CollisionSpec(4, 0.001f);
            projectileProfile.CollisionSpec.AddEntry(MeterType.Energy, -100);
            projectileProfile.VelocityInertia = 0.8f; //??
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0); //??
            return projectileProfile;
        }
    }
}
