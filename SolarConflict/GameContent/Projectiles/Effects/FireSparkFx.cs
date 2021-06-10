using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class FireSparkFx
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
          //  profile.Lights = new ILight[] { Lights.ContactLight(Color.Orange) };
            profile.VelocityInertia = 1;
            profile.RotationInertia = 1;
            profile.TextureID = "add5";
            profile.CollisionWidth = 32;
            profile.InitMaxLifetimeID = "20";
            profile.InitSizeID = "InitFloatRandom,10,10";
            profile.UpdateSize = new UpdateSizeGrow(1);
            profile.ColorLogicID = "FadeInOut"; //maybe FadeinOut
            profile.CollisionType = CollisionType.Effects;
            profile.DrawType = DrawType.Additive;
            profile.Flags = GameObjectFlags.AddOnlyOnScreen;
            return profile;
        }
    }
}
