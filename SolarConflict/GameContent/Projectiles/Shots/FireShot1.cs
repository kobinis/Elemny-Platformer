using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Shots
{
    class FireShot1
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;            
            profile.TextureID = "add7";
            profile.Draw = new ProjectileDrawRotateWithTime(-0.11f, 0.1f, "add7", "add7");
            profile.InitSizeID = "30";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name; //TODO: change
            profile.TimeOutEmitterID = typeof(EmitterImpactFx1).Name; //TODO: change
            profile.CollisionSpec = new CollisionSpec(40, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;            
            return profile;
        }
    }
}
