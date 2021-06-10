using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles
{
    class Astro1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "Astro1";
            profile.TextureID = "spacerock10000";
            profile.InitSize = new InitFloatRandom(100, 10);
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;

            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            ProjectileDrawAni draw = new ProjectileDrawAni();
            for (int i = 10000; i <= 10178; i += 2) 
            {
                draw.AddTextureId("spacerock" + 10000);
            }
            profile.Draw = draw;
            return profile;
        }
    }
}
