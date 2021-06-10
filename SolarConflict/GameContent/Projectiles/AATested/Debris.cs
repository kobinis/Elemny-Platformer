using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    class Debris
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            //projectileProfile.CollusionType = CollusionType.Collide1;// 
            projectileProfile.CollisionType = CollisionType.UpdateOnlyOnScreen;
            projectileProfile.DrawType = DrawType.Alpha;            
            projectileProfile.TextureID = "debris1";
            projectileProfile.ScaleMult = 1f / (float)(projectileProfile.Sprite.Width - 6);
            projectileProfile.InitSize = new InitFloatRandom(5, 5);
            projectileProfile.UpdateSize = null;
            projectileProfile.InitMaxLifetime = new InitFloatConst(200);//texture from param
            projectileProfile.CollisionSpec = new CollisionSpec(0, 0.05f);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.IsEffectedByForce = true;
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            projectileProfile.VelocityInertia = 0.99f;
            projectileProfile.IsTurnedByForce = true;
            projectileProfile.RotationInertia = 0.99f;
            projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;
          //  projectileProfile.InitColor = new InitColorConst(new Color(255, 255, 255, 1));

            return projectileProfile;
        }
    }
}
