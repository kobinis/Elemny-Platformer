using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class AttractiveShock
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.CollideAll;
            profile.DrawType = DrawType.Alpha;
            //profile.UpdateColor = ProjectileProfile.ColorUpdate.f;
            profile.TextureID = "shockwave2";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            
            /*profile.InitSizeId = "40";
            profile.UpdateSize = new UpdateSizeGrow(5, 1.1f);*/

            profile.InitSizeID = "1000";
            //profile.UpdateSize = new UpdateSizeGrow(-5, 1f);

            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0);
            
            profile.InitMaxLifetime = new InitFloatConst(30);
            profile.Mass = 0.1f;
            //profile.ImpactEmitterId = "EmitterImpactFx1";
            profile.CollisionSpec = new CollisionSpec(0, -0.5f);
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.Item;
            return profile;
        }
    }
}
