using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Projectiles
{
    /// <summary>
    /// 
    /// </summary>
    class NuclearBomb
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.AlphaFront;
            //profile.InitColor = new InitColorFaction();
            //profile.ColorLogic = ColorUpdater.FadeOutSlow; //maybe change
            profile.TextureID = "Nuke";
            profile.CollisionWidth = profile.Sprite.Width - 2;
            profile.InitSizeID = "30";            
            profile.InitMaxLifetimeID = "30"; 
            profile.Mass = 0.5f;
                        
            profile.CollisionSpec = new CollisionSpec();            
            profile.CollisionSpec.AddEntry(MeterType.Damage, 1);
            profile.InitHitPointsID = "100"; 
                        
            profile.CollisionType = CollisionType.CollideAll;
            
            profile.IsDestroyedOnCollision = false; //projectile is terminated on impact
            profile.IsEffectedByForce = false;
            profile.VelocityInertia = 0.99f;
            profile.HitPointZeroEmiiterID = "NuclearExplosion";

            var exp = new GroupEmitter();
            exp.AddEmitter("NuclearExplosion");
            exp.AddEmitter("BigExplosionFx");

            profile.TimeOutEmitter = exp;


            return profile;
        }
    }
}
