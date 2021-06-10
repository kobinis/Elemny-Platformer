using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class LaserBit
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(200, 50, 50));
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add10";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "10";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(1);
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(LaserHitFx).Name;            
            profile.CollisionSpec = new CollisionSpec(0.4f, 0f);
            profile.CollisionSpec.AddEntry(MeterType.MiningLevel, 2, ImpactType.Max);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
