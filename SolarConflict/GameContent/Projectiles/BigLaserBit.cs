using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.NewContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles {
    class BigLaserBit {
        public static ProjectileProfile Make() {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(200, 50, 50));
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add10";
            profile.CollisionWidth = profile.Sprite.Width - 10; // why -10?
            profile.InitSizeID = "100";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(1);
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(LaserHitFx).Name; // TODO: change effect
            profile.CollisionSpec = new CollisionSpec(4f, 0f);            
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;

           // profile.ApplyTags("energy", "large"/*, "bright"*/);

            return profile;
        }
    }
}
