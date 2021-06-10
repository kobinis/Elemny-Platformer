using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Projectiles.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class BloodFx1
    {
        public static ProjectileProfile Make()
        {
            var lifetime = 10;
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.AlphaFront;
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;
            projectileProfile.TextureID = "Blood1";//"add8");
            projectileProfile.ScaleMult = 2.5f / (float)(projectileProfile.Sprite.Width);
            projectileProfile.InitSize = new InitFloatRandom(12, 8);
            projectileProfile.UpdateSize = new UpdateSizeGrowMult(1.1f);
            projectileProfile.InitMaxLifetime = new InitFloatRandom(lifetime, 10);
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;
        }
    }
}
