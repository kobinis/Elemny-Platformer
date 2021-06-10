using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// A transparent pushing particle  
    /// </summary>
    class PushingAura
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.None;            
            profile.InitSize = new InitFloatParentSize(1.1f, 100);
            profile.InitMaxLifetimeID = "1000";                        
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.Force = 8;
            profile.CollisionSpec.ForceType = ForceType.FromCenter;
            profile.IsDestroyedWhenParentDestroyed = true; 
            profile.CollisionType = CollisionType.CollideAll;
            profile.CollideWithMask = GameObjectType.Agent | GameObjectType.Asteroid | GameObjectType.Item;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;            
            return profile;
        }
    }
}
