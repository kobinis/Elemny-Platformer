using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict
{
    class ProjFxSmoke1
    {
        public static void Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.ID = "ProjFxSmoke1";
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.AlphaFront;
            projectileProfile.ColorLogic = new UpdateColorFade(0.2f, -0.2f); //ProjectileProfile.ColorUpdate.FadeOut;
            projectileProfile.TextureID = "smoke2";
            projectileProfile.ScaleMult = 1f / (float)projectileProfile.Sprite.Width;
            projectileProfile.InitSize = new InitFloatParentSize(); //new InitFloatRandom(50, 5);
            projectileProfile.UpdateSize = new UpdateSizeGrow(4);
            projectileProfile.InitMaxLifetime = new InitFloatConst(50);
            projectileProfile.InitColor = new InitColorConst(new Color(255, 255, 255, 100));

            ContentBank.Inst.AddContent(projectileProfile);
            ContentBank.Inst.AddContent(projectileProfile);
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
        }
    }
}
