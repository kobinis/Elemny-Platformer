using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class RammingFront
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.None;
            //profile.InitColor = new ;            
            profile.TextureID = "add3";            
            profile.InitSizeID = "40";            
            profile.InitMaxLifetimeID = "1";
            profile.Mass = 0.5f;
            profile.ImpactEmitterID = "CollisionDebris";    //Change       
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.Flags |= CollisionSpecFlags.IsNotPushingAllies;
            profile.CollisionSpec.Force = 8;
            profile.CollisionSpec.ForceType = ForceType.Rotation; //??
            //profile.CollusionSpec.AddEntry(MeterType.StunTime, 0.5f, ImpactType.Max);
            profile.CollisionSpec.AddEntry(MeterType.Damage, 0.9f, ImpactType.Velocity);            
            profile.IsDestroyedWhenParentDestroyed = true; // maybe change it to lose hitpoint on parent
            profile.CollisionType = CollisionType.Collide1;
            profile.CollideWithMask = GameObjectType.Agent | GameObjectType.Asteroid;              
            profile.IsDestroyedOnCollision = false; //waybeTrue
            profile.IsEffectedByForce = false;            
            //Movement                       
            return profile;
        }
    }
}
