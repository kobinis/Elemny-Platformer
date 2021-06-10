using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class PlasmaShot1
    {
        /// <summary>
        /// A Plasma shot - 40 damage, 100 life time;
        /// </summary>
        /// <returns></returns>
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add4";
            profile.Draw = new ProjectileDrawRotateWithTime(-0.11f, 0.1f, "add4", "add4");            
            profile.InitSizeID = "15";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name; //TODO: change
            profile.CollisionSpec = new CollisionSpec(40, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }
    }
}
