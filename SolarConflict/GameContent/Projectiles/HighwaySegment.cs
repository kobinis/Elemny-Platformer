using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class HighwaySegment
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.CollisionType = CollisionType.UpdateOnlyOnScreen;
            profile.DrawType = DrawType.Additive;
            profile.InitColor = new InitColorConst(new Color(255, 255, 255, 100));
            //Update color sin
            profile.TextureID = "highway";
            profile.CollisionWidth = profile.Sprite.Width;
            profile.InitSizeID = "300";
            profile.UpdateSize = null;
            profile.Mass = 0.1f;
            //  profile.ImpactEmitterID = "EmitterPickupFx"; //?? add cooldown
           // profile.Draw = new ProjectileDrawRotateWithTime(0.001f, 0.0011f, "add14");
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.ForceType = ForceType.DirectionOfMovment;
            profile.CollisionSpec.Force = 0.7f;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.CollideWithMask = GameObjectType.Ship | GameObjectType.Asteroid | GameObjectType.Item;
            return profile;
        }
    }
}

