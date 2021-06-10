using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class ShotTemplate
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "add10";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "10";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name;
            
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            //profile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.8f, 15); 
            //profile.UpdateRotation = new UpdateRotationForward();
            profile.CollisionSpec = new CollisionSpec(10,0.5f);
            profile.CollisionSpec.AddEntry(MeterType.StunTime, 60, ImpactType.Max);
            profile.CollisionSpec.AddEntry(MeterType.Energy, -20); //takes 20 energy
            profile.CollisionSpec.AddEntry(MeterType.Shield, -20); //takes 20 Shield
            profile.CollisionSpec.AddEntry(MeterType.Hitpoints, -20); //takes 20 hitpoint, ignores Shield

            return profile;
        }
    }
}
