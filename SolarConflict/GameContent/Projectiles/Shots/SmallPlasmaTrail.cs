using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots
{
    class SmallPlasmaTrail
    {
        public static IEmitter Make()
        {
            ProjectileProfile profile = new ProjectileProfile();

            // Appearance
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(0.5f, 1f, 0.5f));
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "glow128";     //add2       
            profile.CollisionWidth = profile.Sprite.Width - 28;
            profile.InitSize = new InitFloatConst(16);
            profile.UpdateSize = new UpdateSizeGrow(0, 0.991f);

            // Logic
            //profile.Mass = 0.1f;
            profile.IsEffectedByForce = false;
            profile.VelocityInertia = 0f;


            profile.InitMaxLifetime = new InitFloatConst(Utility.Frames(3.5f));
            profile.CollisionSpec = new CollisionSpec(20f, 0f);
            //profile.CollisionSpec.AffectsAllies = true;

            //  profile.ApplyTags(new Vector3(0.17f, 0.459f, 1f), "trail", "energy", "bright");

            return profile;
        }
    }
}
