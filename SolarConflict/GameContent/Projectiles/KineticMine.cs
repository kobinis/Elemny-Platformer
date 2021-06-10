using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    class KineticMine
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();            
            profile.DrawType = DrawType.AlphaFront;
            profile.InitColor = new InitColorFaction();
            profile.ColorLogic = ColorUpdater.FadeOutSlow; //maybe change
            profile.TextureID = "DamageWarhead";
            profile.CollisionWidth = profile.Sprite.Width - 2;
            profile.InitSizeID = "30";
            profile.UpdateSize = null; // new UpdateSizeGrow(1.1f);
            profile.InitMaxLifetimeID = "600";  // 1/60 of a second
            profile.Mass = 1f;
            //profile.ImpactEmitterId = "EmitterImpactFx1"; //maybe sound
            profile.CollisionSpec = new CollisionSpec();
            //profile.ImpactSpec.AddEntry(MeterType.StunTime, 10, ImpactType.Max);  cedf
            profile.CollisionSpec.AddEntry(MeterType.Damage, 0.3f, ImpactType.Velocity);
            profile.InitHitPointsID = "1000"; //check it
            profile.IsDestroyedWhenParentDestroyed = false; 
            profile.CollisionType = CollisionType.CollideAll; //??

            profile.IsDestroyedOnCollision = false; //projectile is terminated on impact
            profile.IsEffectedByForce = true;
            profile.Draw = new ProjectileDrawRotateWithTime(0.1f, 0.1f, "DamageWarhead"); //  change
            profile.VelocityInertia = 0.995f;
            profile.ObjectType = GameObjectType.Projectile | GameObjectType.PhysicalProjectile;
            return profile;
        }
    }
}
