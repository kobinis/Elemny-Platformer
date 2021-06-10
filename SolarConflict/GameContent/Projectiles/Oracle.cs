using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;
using SolarConflict.GameContent;

namespace SolarConflict.NewContent.Projectiles
{
    class Oracle
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Lit; //Add alpha back
            //profile.ColorLogic = ColorUpdater.FadeInOut;
            //profile.InitColor = ;
            profile.TextureID = "oracle";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "1000";
            //   profile.UpdateSize = new UpdateSizeGrow(0, 1.005f);
            // profile.InitMaxLifetime = new InitFloatConst(100);
            profile.Mass = 1f;
            //     profile.ImpactEmitterID = "SmokeExplosionFx";
            //    profile.UpdateSize = new UpdateSizeGrow(1, 1f);
            //profile.ImpactEmitterId = "FireExplosionFx";
            //    profile.CollisionSpec = new CollisionSpec(80, 0.5f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;
            profile.CollideWithMask = GameObjectType.None;

            //profile.ApplyTags("explosion", "medium");

            return profile;
        }
    }
}

