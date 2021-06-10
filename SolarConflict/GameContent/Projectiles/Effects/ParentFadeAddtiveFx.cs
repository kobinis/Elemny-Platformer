using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Effects
{
    class ParentFadeAddtiveFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;//ProjectileProfile.ColorUpdate.FadeInOut;
            projectileProfile.Sprite = null;
            projectileProfile.ScaleMult = 1;
            projectileProfile.InitSize = new InitFloatConst(5); // maybe change to parent
            projectileProfile.UpdateSize = null;
            projectileProfile.Draw = new ProjectileDrawParent();
            projectileProfile.InitMaxLifetime = new InitFloatConst(40);
            projectileProfile.InitColor = new InitColorParent();
            projectileProfile.RotationLogic = new UpdateRotationParent();
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;
        }
    }
}
