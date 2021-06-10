using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class ItemPullingAoe
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.CollideAll;
            profile.DrawType = DrawType.Alpha;
            profile.ColorLogic = null;// new UpdateColorFadeInOut();
            profile.TextureID = "shields1";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "1000";
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            profile.InitMaxLifetime = new InitFloatConst(30);
            profile.Mass = 0.1f;
            profile.CollisionSpec = new CollisionSpec(0, -0.5f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.Item | GameObjectType.Collectible;
            profile.DrawType = DrawType.None;
            return profile;
        }
    }
}
