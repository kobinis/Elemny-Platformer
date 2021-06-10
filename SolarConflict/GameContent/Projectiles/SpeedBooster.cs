using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class SpeedBooster
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;
            profile.DrawType = DrawType.Alpha;
            profile.InitColor = new InitColorConst(new Color(255, 255, 255, 100)); 
            //Update color sin
            profile.TextureID = "arrow";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "165";
            profile.UpdateSize = null;           
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = "EmitterPickupFx"; //?? add cooldown
            profile.ImpactEmitterCooldownTime = 3;
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.ForceType = ForceType.Rotation;
            profile.CollisionSpec.Force = 0.7f;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.Ship | GameObjectType.Asteroid | GameObjectType.Item;
            return profile;
        }
    }
}
