using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class Wall
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ID = "Wall";
            profile.DrawType = DrawType.Additive;
            profile.CollisionType = CollisionType.CollideAll;
            profile.TextureID = "glow128"; //??
            profile.CollisionWidth = profile.Sprite.Width + 100;
            profile.InitSizeID = "60";
            profile.Mass = 0.2f;
            profile.InitColor = new InitColorConst( new Color(200, 200, 255));
            //profile.HitPointZeroEmiiterId = "SelfDestruct";
            //profile.InitHitPointsId = "30";
            profile.VelocityInertia = 0.99f;
            profile.CollisionSpec = new CollisionSpec(0, 100f);
            profile.IsDestroyedOnCollision = false;          
            profile.IsEffectedByForce = false;
            profile.IsTurnedByForce = false;
            return profile;
        }
    }
}
