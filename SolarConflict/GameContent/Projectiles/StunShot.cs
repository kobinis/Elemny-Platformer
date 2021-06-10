using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class StunShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();            
            //profile.InitColor = new InitColorConst(new Color(255,255,100));
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add3";
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitSizeID = "10";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = "EmitterImpactFx1";
            profile.CollisionSpec = new CollisionSpec(0, 0.5f, 60);
            profile.CollisionSpec.AddEntry(MeterType.Energy, -10);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
