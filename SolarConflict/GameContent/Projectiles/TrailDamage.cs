using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class TrailDamage
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(0.5f, 0.5f, 0.5f));
            profile.ColorLogic = ColorUpdater.FadeOut;
            profile.TextureID = "glow128";     //add2       
            profile.CollisionWidth = profile.Sprite.Width - 30;
            profile.InitSizeID = "40";
           // profile.UpdateSize = new UpdateSizeGrow(-0.25f, 1, 1000, 20);
            
            profile.InitMaxLifetime = new InitFloatConst(Utility.Frames(1f));
            profile.Mass = 0.1f;
            profile.VelocityInertia = 0f;
            profile.CollisionSpec = new CollisionSpec(1f, 0.01f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;          
            return profile;
        }
    }
}
