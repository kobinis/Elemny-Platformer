using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class NovaRingExplosion
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(0.5f, 0.5f, 0.5f));
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "bigShockwave";
            //profile.Draw = new ProjectileDrawRotateWithTime(0.02f,0.022f);
            profile.CollisionWidth = profile.Sprite.Width - 5;
            profile.InitSizeID = "1500";
            profile.UpdateSize = new UpdateSizeGrow(4, 1.015f);
            profile.InitMaxLifetime = new InitFloatConst(150);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0.9f; //TODO: maybe remove
            profile.CollisionSpec = new CollisionSpec(10f, 10f); //1
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            // profile.Draw = new ProjectileDrawRotateWithTime(0.01f,-0.015f, null, 1, 1);
            //
            //profile.InitParam = new InitFloatRandom(0, 1000); //maybe remove
            //rotation 
            profile.VelocityInertia = 0.8f;
            return profile;
        }
    }
}
