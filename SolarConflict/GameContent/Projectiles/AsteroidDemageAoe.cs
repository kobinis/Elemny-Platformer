using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class AsteroidDemageAoe
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.CollideAll;
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(150,90,20));
            //profile.UpdateColor = ProjectileProfile.ColorUpdate.f;
            profile.TextureID = "add11";// "shockwave2";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSize = new InitFloatParentSize(1.5f,60f);
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            profile.RotationLogic = new UpdateRotationParent();
            profile.InitMaxLifetime = new InitFloatConst(30);
            profile.Mass = 0.1f;
            profile.CollisionSpec = new CollisionSpec(90f, 0f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            //profile.Draw = new ProjectileDrawRotateWithTime(0.01f, -0.01f, "add11");
            profile.CollideWithMask = GameObjectType.Asteroid;
            profile.CollisionSpec.Flags = CollisionSpecFlags.AffectsAllies;
            return profile;
        }
    }
}
