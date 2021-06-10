using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class EnergyDrainAoe
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
            projectileProfile.UpdateSize = new UpdateSizeGrow(15);
            projectileProfile.InitMaxLifetime = new InitFloatConst(60);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(Color.LightBlue);
            projectileProfile.CollisionSpec = new CollisionSpec();
            projectileProfile.CollisionSpec.AddEntry(MeterType.Energy, -1);
            projectileProfile.VelocityInertia = 0.8f;
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            return projectileProfile;
        }
    }
}
