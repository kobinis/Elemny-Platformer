using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class StarDestroyerShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOut;
            profile.TextureID = "add15";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "70";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(60*5);
            profile.Mass = 0.5f;            
            profile.CollisionSpec = new CollisionSpec(3, 0.5f);
            profile.InitHitPointsID = "500";
            profile.HitPointZeroEmiiterID = typeof(HugeDamageShot).Name;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false; // Maybe change to true
            //profile.Draw = new ProjectileDrawRotateWithTime(0.01f, -0.015f, "add14", "add14");
            return profile;
        }
    }
}
