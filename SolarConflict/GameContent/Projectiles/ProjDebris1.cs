using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class ProjDebris1
    {        
        public static ProjectileProfile Make()
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();   
            //projectileProfile.CollusionType = CollusionType.Collide1;// 
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Alpha;
            projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;//ProjectileProfile.ColorUpdate.FadeInOut;
            projectileProfile.TextureID = "debris1";
            projectileProfile.ScaleMult = 1f / (float)(projectileProfile.Sprite.Width-6);
            projectileProfile.InitSize = new InitFloatRandom(5, 5);
            projectileProfile.UpdateSize = null;
            projectileProfile.InitMaxLifetime = new InitFloatConst(100);//texture from param
            projectileProfile.CollisionSpec = CollisionSpec.SpecForce;
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.IsEffectedByForce = true;
            projectileProfile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return projectileProfile;   
        }
    }
}