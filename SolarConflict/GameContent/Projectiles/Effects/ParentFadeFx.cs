using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{    
    class ParentFadeFx //TODO: change name, add scale or maybe parent draw
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Alpha;
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
