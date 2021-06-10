using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{   
    class DamageAoe
    {
        public static ProjectileProfile Make()
        {
            return MakeWithTweaks(typeof(DamageAoe).Name);
        }        

        public static ProjectileProfile MakeWithTweaks(string id) {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            //profile.InitColor = new InitColorConst(new Color(0.5f, 0.5f, 0.5f));
            profile.ColorLogic = ColorUpdater.FadeInOut;
            profile.TextureID = "add2";
            
            profile.CollisionWidth = profile.Sprite.Width - 5;
            profile.InitSize = new InitFloatConst(15f);
            profile.UpdateSize = new UpdateSizeGrow(4, 1f);
            profile.InitMaxLifetime = new InitFloatConst(50);
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0.8f;
            profile.CollisionSpec = new CollisionSpec(1f, 0f); //1
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.Draw = new ProjectileDrawRotateWithTime(0f, 0f, "add2");
            
            return profile;
        }
    }
}
