using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Effects
{
    class GenericTrail
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.InitColor = new InitColorParent();
            profile.InitSize = new InitFloatParentSize();
            profile.VelocityInertia = 0.9f;
            profile.TextureID = "add8Gray";
            //profile.InitSizeID = "20";
            profile.InitMaxLifetimeID = "20";
            profile.ColorLogicID = "FadeOut";
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.Additive;
            profile.Draw = new ProjectileDrawRotateWithTime(0.1f, 0.102f, "add8", "add8");
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
