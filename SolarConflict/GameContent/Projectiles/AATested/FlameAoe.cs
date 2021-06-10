using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    class FlameAoe
    {
        public static ProjectileProfile Make()
        {            
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(0.5f, 0.5f, 0.5f));
            profile.ColorLogic = ColorUpdater.FadeOut;
            profile.TextureID = "add2";
            profile.Draw = new ProjectileDrawRotateWithTime(0f, 0f, "add2");

            profile.CollisionWidth = profile.Sprite.Width - 5;
            profile.InitSizeID = "15";
            profile.UpdateSize = new UpdateSizeGrow(4, 1.01f);
            profile.InitMaxLifetime = new InitFloatConst(50);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0.99f;
            profile.CollisionSpec = new CollisionSpec(1f, 0f); //1
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            //profile.ApplyTags("effect", "explosion", "medium");
          //  profile.ApplyTags("energy", "medium");
            profile.Light = Lights.MediumLight(new Color(255, 180, 60));
            return profile;
        }
    }
}
