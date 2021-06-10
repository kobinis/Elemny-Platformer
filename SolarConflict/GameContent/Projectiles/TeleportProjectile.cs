using SolarConflict.Framework.GameObjects.Emitters;
using SolarConflict.GameContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// Find a place for you to teleport 
    /// </summary>
    class TeleportProjectile
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();            
            profile.CollisionType = CollisionType.CollideAll;            
            profile.DrawType = DrawType.None;            
            profile.TextureID = "circle64";            
            profile.InitSize = new InitFloatParentSize();            
            profile.InitMaxLifetime = new InitFloatConst(30);
            profile.Mass = 2f;            
            profile.TimeOutEmitterID = typeof(TeleportWithEffect).Name;
            profile.CollisionSpec = new CollisionSpec(0,0);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.VelocityInertia = 0.9f;
                        
            return profile;
        }
    }
}
