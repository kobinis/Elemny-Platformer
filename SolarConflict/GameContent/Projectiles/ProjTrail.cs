using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict
{
    class ProjTrail
    {
        public static void Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.ID = "Trail";
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = new UpdateColorFade(0.2f, -0.2f); //ProjectileProfile.ColorUpdate.FadeOut;
            projectileProfile.TextureID = "lightGlow";
            projectileProfile.ScaleMult = 1f / (float)projectileProfile.Sprite.Width;
            projectileProfile.InitSize = new InitFloatConst(80);
            projectileProfile.InitMaxLifetime = new InitFloatConst(500);

            projectileProfile.UpdateEmitter = (IEmitter)ContentBank.Inst.GetEmitter("FxTrail1");
            projectileProfile.UpdateEmitterCooldownTime = 3;

            ContentBank.Inst.AddContent(projectileProfile);
        }
    }
}

