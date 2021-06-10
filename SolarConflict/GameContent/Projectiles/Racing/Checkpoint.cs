using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles.Racing
{
    class Checkpoint
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Alpha;            
            profile.TextureID = "ball128";             
            profile.InitSizeID = "512";                        
            profile.Mass = 1000;
            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name;            
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = false;                                                            
            return profile;
        }
    }
}
