using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Projectiles.Effects
{
    class ExpParticleParentSizedFx1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.InitColor = new InitColorConst(Color.LightGray);
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;
            projectileProfile.TextureID = "add2";
            //projectileProfile.ScaleMult = 1f / (float)projectileProfile.Sprite.Width;
            projectileProfile.InitSize = new InitFloatParentSize(0.2f, 0.1f);
            projectileProfile.UpdateSize = /*new UpdateSizeGrowMult(1.1f);*/ new UpdateSizeGrow(0, 1.03f);//
            projectileProfile.InitMaxLifetime = new InitFloatParentSize(0.3f,10); //new InitFloatConst(15);
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;
        }
    }
}
