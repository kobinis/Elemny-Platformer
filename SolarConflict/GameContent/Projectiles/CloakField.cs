using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class CloakField
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "CloakField";
            profile.DrawType = DrawType.Alpha;
            profile.InitColor = new InitColorConst(Color.LightBlue);
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "shockwave2";
            profile.CollisionWidth = profile.Sprite.Width ;
            profile.InitSizeID = "1000";           
            profile.InitMaxLifetime = new InitFloatConst(2);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0.9f;
            profile.CollisionSpec.AddEntry(MeterType.Cloak, 4, ImpactType.Max);
            profile.CollisionSpec.Force = 0;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            return profile;
        }
    }
}
