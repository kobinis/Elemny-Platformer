using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class GreenSun
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.ObjectType |= GameObjectType.Sun | GameObjectType.VisibleInMiniMap;
            profile.CraftingStationType = Framework.CraftingStationType.Sun;
            //display
            profile.DrawType = DrawType.Additive;
            profile.TextureID = "sun";
            var draw = new ProjectileDrawRotateWithTime(-0.001f, 0.0011f, "greensun", "greensun2", ProjectileProfile.WidthToScale(profile.Sprite.Width - 60), ProjectileProfile.WidthToScale(profile.Sprite.Width - 60));
            // draw.DisplayScaleMult = profile.TextureProxy.Width - 100;
            //  draw.DisplayScale = profile.TextureProxy.Width- 30;
            profile.Draw = draw;
            profile.CollisionWidth = profile.Sprite.Width - 90;
            
            // TODO: authentically white sun? It'd make our game distinct.

            profile.InitSizeID = "5000";
            profile.HitPointZeroEmiiterID = "HugeExplosionEmitter";

            profile.CollisionSpec = new CollisionSpec(10f, 8f);
            profile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies;
            profile.Mass = 1000;

            profile.RotationMass = 10;

            profile.InitHitPointsID = "15000";
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.IsTurnedByForce = false;
            profile.RotationInertia = 0.99f;
            profile.VelocityInertia = 0.99f;
            //profile.IsPotentialTarget = true; - kobi: bring back to use it in HudMap
            profile.UpdateEmitterCooldownTime = 200;
            //  profile.UpdateEmitterID = "PushingAura";

            UpdateHitPoints updateHitPoints = new UpdateHitPoints(10, 0, 15000);

            profile.UpdateList.Add(updateHitPoints);

            profile.CollisionType = CollisionType.CollideAll;
            PointLight light = new PointLight(Color.LightGreen.ToVector3(), 1000000, 1);
            
            profile.Light = light;
            return profile;
        }
    }
}
