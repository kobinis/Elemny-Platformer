using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class SelfDestruct //change name
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = MethodBase.GetCurrentMethod().DeclaringType.Name;
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOut;
            profile.TextureID = "add1";
            profile.CollisionWidth = profile.Sprite.Width - 5;
            profile.InitSizeID = "10";
            profile.UpdateSize = new UpdateSizeGrow(3, 1.02f);
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0.9f;
            profile.CollisionSpec = new CollisionSpec(2f, -0.01f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
