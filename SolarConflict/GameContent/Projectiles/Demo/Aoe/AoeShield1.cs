using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// Regenerates shields in an area
    /// </summary>
    class AoeShield1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.Collide1;
            profile.DrawType = DrawType.Additive;
            profile.TextureID = "shockwave2";
            profile.InitColor = new InitColorConst(Color.Blue);
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "900";
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            profile.InitMaxLifetime = new InitFloatConst(60);
            profile.Mass = 0.1f;
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.AddEntry(MeterType.Shield, 1f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            //profile.CollideWithMask = GameObjectType.Agent;
            return profile;
        }
    }
}
