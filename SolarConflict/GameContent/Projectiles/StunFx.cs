using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class StunFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "StunFx";
            profile.TextureID = "4";
            profile.CollisionWidth = TextureBank.Inst.GetTexture("4").Width;
            profile.InitSize = new InitFloatParentSize(1.3f, 0);
            profile.MovementLogic = new MoveWithParent();
            profile.CollisionType = CollisionType.Effects;
            profile.InitMaxLifetimeID = "11";
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            ProjectileDrawAni draw =  new ProjectileDrawAni();
            for (int i = 1; i <= 8; i++)
            {
                draw.AddTextureId(i.ToString());
            }
            draw.globalTimeMult = 0.1f;
            profile.Draw = draw;
            return profile;
        }
    }
}
