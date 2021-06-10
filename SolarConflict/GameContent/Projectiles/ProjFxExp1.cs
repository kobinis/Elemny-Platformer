using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict
{
    class ProjFxExp1
    {
        public static void Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.ID = "FxExp1";
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;
            projectileProfile.TextureID = "exp1";
            projectileProfile.ScaleMult = 1f / (float)projectileProfile.Sprite.Width;
            projectileProfile.InitSize = new InitFloatRandom(5, 4);
            projectileProfile.UpdateSize = /*new UpdateSizeGrowMult(1.1f);*/ new UpdateSizeGrow(4);//
            projectileProfile.InitMaxLifetime = new InitFloatConst(15);
            ContentBank.Inst.AddContent(projectileProfile);
            ContentBank.Inst.AddContent(projectileProfile);
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
        }
    }
}
