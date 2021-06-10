using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class ExplosionParticleFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();            
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeOut;
            projectileProfile.TextureID = "add2";// "exp1";
            projectileProfile.ScaleMult = 1f / (float)projectileProfile.Sprite.Width;
            projectileProfile.InitSize = new InitFloatRandom(5, 4);
            projectileProfile.UpdateSize = /*new UpdateSizeGrowMult(1.1f);*/ new UpdateSizeGrow(4);//
            projectileProfile.InitMaxLifetime = new InitFloatConst(15);

            //  projectileProfile.ApplyTags("explosion", "small");
            // ^ PERFORMANCE WARNING: this could get ugly. Might wanna drop this, have effects which spawn these particles randomly
            // assign lights to some small fraction of them
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;

            return projectileProfile;
        }
    }
}
