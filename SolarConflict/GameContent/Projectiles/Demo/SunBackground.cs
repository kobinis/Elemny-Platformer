
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
    {
        class SunBackground
        {
            public static ProjectileProfile Make()
            {
                ProjectileProfile profile = new ProjectileProfile();
                profile.ObjectType |= GameObjectType.Sun;
                profile.CraftingStationType = Framework.CraftingStationType.Sun;
                //display
                profile.DrawType = DrawType.Alpha;
                profile.TextureID = "SunBackground";
                var draw = new ProjectileDrawRotateWithTime(-0.001f, 0.0011f, "SunBackground");
                //profile.CollisionWidth = profile.TextureProxy.Width - 280;
                //draw.DisplayScale = profile.TextureProxy.Width - 280;
                profile.InitColor = new InitColorConst(new Microsoft.Xna.Framework.Color(100,100,100));

                profile.InitSizeID = "5000";
                profile.HitPointZeroEmiiterID = "SunExplosion";

                profile.CollisionSpec = new CollisionSpec(100f, 0.1f);
                profile.Mass = 1000;

                profile.RotationMass = 10;
                profile.InitHitPointsID = "15000";
                profile.IsDestroyedOnCollision = false;
                profile.IsEffectedByForce = false;                
                
                profile.CollisionType = CollisionType.Collide1;
                profile.CollideWithMask = GameObjectType.None;
                return profile;
            }
        }
    }


