using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots {
    class FlakExplosion2Shrapnel {
        public static ProjectileProfile Make() {
            ProjectileProfile projectileProfile = new ProjectileProfile();

            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;
            projectileProfile.TextureID = "exp1";
            projectileProfile.ScaleMult = 1f / (float)projectileProfile.Sprite.Width;
            projectileProfile.InitSize = new InitFloatRandom(5, 4);
            projectileProfile.UpdateSize = new UpdateSizeGrow(4);
            projectileProfile.InitMaxLifetime = new InitFloatConst(15);
            
            projectileProfile.Mass = 0.1f;
            projectileProfile.CollisionSpec = new CollisionSpec(1.5f, 0.1f);

            return projectileProfile;
        }
    }
}
