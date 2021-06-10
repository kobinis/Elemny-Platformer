using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NewContent.Projectiles
{
    static class FirstGateLeft
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.AlphaFront;
            profile.TextureID = "firstgate";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "1000";
            profile.ScaleMult = 2.5f/(float)profile.Sprite.Height;
            profile.Mass = 100f;
            profile.CollisionSpec = new CollisionSpec(0, 10);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;
            profile.CollideWithMask = GameObjectType.Agent;
            return profile;
        }
    }
}
