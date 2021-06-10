using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class KineticShot1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "KineticShot1";
            //profile.InitColor = new InitColorConst(new Color(255,255,100));
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "shot2";
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitSizeID = "10";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = "EmitterImpactFx1";
            profile.CollisionSpec = new CollisionSpec(0.5f, ImpactType.Velocity, 0.5f); //1
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
