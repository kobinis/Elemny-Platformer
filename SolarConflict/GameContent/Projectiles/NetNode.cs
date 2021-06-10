using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class NetNode
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "lightGlow";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "15";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "600";
            profile.Mass = 1f;
            //profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name;            
            profile.CollisionSpec = new CollisionSpec();
            profile.CollisionSpec.Force = 0.1f;
            profile.CollisionSpec.ForceType = ForceType.Mult;
            profile.VelocityInertia = 0.6f;

            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            return profile;
        }
    }
}
