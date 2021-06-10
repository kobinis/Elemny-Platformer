using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class FxSpark
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.VelocityInertia = 1;
            profile.RotationInertia = 1;
            profile.TextureID = "add6";
            profile.CollisionWidth = 32;
            profile.InitMaxLifetimeID = "20";
            profile.InitSize = new InitFloatRandom(10, 10); //"InitFloatRandom,10,10";
            profile.ColorLogicID = "FadeInOut";
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.Additive;
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
