using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Effects
{
    class RedLightFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = "light";
            // projectileProfile.ScaleMult = 1f / (float)(projectileProfile.Sprite.Width - 6) * 2.2f;
            projectileProfile.InitSize = new InitFloatConst(10);
            projectileProfile.UpdateSize = null;
            projectileProfile.InitMaxLifetime = new InitFloatConst(2);//texture from param
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0); //new MoveWithParent();
            projectileProfile.InitColor = new InitColorConst(Color.White);
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            projectileProfile.Light = new PointLight(Color.Red.ToVector3(), 1000, 3);
            return projectileProfile;
        }
    }
}
