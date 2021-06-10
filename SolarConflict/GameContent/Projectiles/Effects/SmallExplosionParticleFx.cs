using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles {
    class SmallExplosionParticleFx {
        public static ProjectileProfile Make() {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;
            projectileProfile.TextureID = "exp1";
            projectileProfile.ScaleMult = 1f / (float)projectileProfile.Sprite.Width;
            projectileProfile.InitSize = new InitFloatRandom(5, 4);
            projectileProfile.UpdateSize = /*new UpdateSizeGrowMult(1.1f);*/ new UpdateSizeGrow(4);//
            projectileProfile.InitMaxLifetime = new InitFloatConst(15);

            //  projectileProfile.ApplyTags(new Vector3(1f, 0.85f, 0.33f), "explosion", "tiny");
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;
        }
    }
}
