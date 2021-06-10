using Microsoft.Xna.Framework;
using SolarConflict.GameContent;
using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{   
    class SunWithBackground
    {                       
        public static ProjectileProfile Make()
        {
            var profile = new ProjectileProfile();
            profile.ObjectType |= GameObjectType.Sun | GameObjectType.VisibleInMiniMap;
            profile.CraftingStationType = Framework.CraftingStationType.Sun;
            //display
            profile.DrawType = DrawType.Alpha;
            profile.TextureID = "SunBackground";
            var draw = new ProjectileDrawRotateWithTime(-0.001f, 0.0011f, "SunBackground", null, ProjectileProfile.WidthToScale(660)); //520            
            profile.Draw = draw;
            profile.InitColor = new InitColorConst(new Color(255, 255, 255, 250));

            profile.InitSizeID = "5000";           
            profile.HitPointZeroEmiiterID = "SunExplosion";

            profile.CollisionSpec = new CollisionSpec(20f, 8f);
            profile.CollisionSpec.Flags = CollisionSpecFlags.AffectsAllies;
            profile.Mass = 1000;

            profile.RotationMass = 10;

            profile.InitHitPointsID = "80000";

            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;
            profile.IsTurnedByForce = false;
            profile.RotationInertia = 0.99f;
            profile.VelocityInertia = 0.99f;
            //profile.UpdateEmitterID = typeof(PushingAura).Name;
            //profile.UpdateEmitterCooldownTime = 20;
            //var ic = new IntensityCalculators.InverseMononomial(0.3);
            profile.Light = new PointLight(Color.LightYellow.ToVector3(), 100000, 5);

            UpdateHitPoints updateHitPoints = new UpdateHitPoints(10, 0, 15000);

            profile.UpdateList.Add(updateHitPoints);

            profile.CollisionType = CollisionType.CollideAll;
            return profile;
        }
    }
}
