using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.NewContent.Emitters;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// Normal shot, 10 damage
    /// </summary>
    class Shot1
    {       
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.Light = Lights.SmallLight(Color.Yellow);      
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;            
            profile.TextureID = "add10";
            profile.CollisionWidth = profile.Sprite.Width-10;            
            profile.InitSizeID = "20";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";            
            profile.Mass = 0.1f;
          //  profile.RotationLogic = new UpdateRotationForward();
            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name;
            profile.CollisionSpec = new CollisionSpec(15, 0.5f);            
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
