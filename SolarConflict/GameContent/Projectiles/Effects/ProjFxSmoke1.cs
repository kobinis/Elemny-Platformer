using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class ProjFxSmoke1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.VelocityInertia = 1;
            profile.RotationInertia = 1;
            profile.TextureID = "smoke2";
            profile.CollisionWidth = 256;
            profile.InitColor = new InitColorConst(new Color(255,255,255,100));
            profile.InitSizeID = "InitFloatRandom,50,50";
            profile.ColorLogicID = "FadeInOut";
            profile.InitMaxLifetimeID = "50";            
            profile.ColorLogic = new UpdateColorFade(0.2f, -0.2f);
            profile.UpdateSize = new UpdateSizeGrow(4);
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.AlphaFront;
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
