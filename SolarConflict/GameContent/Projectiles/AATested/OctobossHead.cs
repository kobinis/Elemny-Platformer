using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.AATested
{
    class OctobossHead
    {
        public static ProjectileProfile Make() //TODO: add sound,
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.AggroRange = 4500;
            profile.DrawType = DrawType.Alpha;
            profile.TextureID = "BlobSegment";
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitHitPointsID = "100";
            profile.InitSizeID = "150";
            profile.InitMaxLifetime = new InitFloatConst(60 * 60 * 2);
            profile.Mass = 0.5f;
            profile.CollisionType = CollisionType.CollideAll;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            profile.InitColor = new InitColorConst(new Color(200, 255, 200, 255));            
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.AnyPotentialTarget, 4f, 4.5f, false); ;
            profile.VelocityInertia = 0.8f;
            profile.RotationLogic = new UpdateRotationForward(); 
            profile.CollisionSpec = new CollisionSpec(2, 20f);
            profile.CollisionSpec.IsDamaging = true;
            profile.ScaleMult = 0.05f;                        
            profile.Draw = new ProjectileDrawScale((Sprite)"BlobHead");            
            //profile.HitPointZeroEmiiterID = "ProjShipwreck1"; //TODO: add
            return profile;
        }
    }
}
