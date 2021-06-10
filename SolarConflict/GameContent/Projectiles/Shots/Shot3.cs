using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class Shot3
    {

        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "Shot3";
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "lightGlow";
            profile.CollisionWidth = profile.Sprite.Width-5;
            profile.InitSizeID = "20";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatConst(100);
            profile.Mass = 0.1f;
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Enemy, 0.1f, 15);
            profile.ImpactEmitterID = "EmitterImpactFx1";
            profile.CollisionSpec = new CollisionSpec(0.1f, ImpactType.Additive, 0.5f);
            profile.CollisionSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Energy, -10));
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
