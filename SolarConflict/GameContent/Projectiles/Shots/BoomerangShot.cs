using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.NewContent.Projectiles
{
    class BoomerangShot
    {
        public static ProjectileProfile Make()
        {
            ProjectileProfile profile = new ProjectileProfile();
            //profile.InitColor = new InitColorConst(new Color(255,255,100));
            profile.DrawType = DrawType.AlphaFront;
            profile.InitColor = new InitColorFaction();
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "boomerang";
            profile.CollisionWidth = profile.Sprite.Width - 2;
            profile.InitSizeID = "24";
            profile.UpdateSize = null; // new UpdateSizeGrow(1.1f);
            profile.InitMaxLifetimeID = "1000";  // 1/60 of a second
            profile.Mass = 0.3f;       
            profile.CollisionSpec = new CollisionSpec();
            //profile.ImpactSpec.AddEntry(MeterType.StunTime, 10, ImpactType.Max);
            profile.CollisionSpec.AddEntry(MeterType.Damage, 1, ImpactType.Velocity);
            profile.InitHitPointsID = "5000"; //check it

            profile.IsDestroyedWhenParentDestroyed = true; // maybe change it to lose hitpoint on parent
            profile.CollisionType = CollisionType.Collide1;
            

            ImpacedEndAndEmmit endLifeEmitter = new ImpacedEndAndEmmit();
            endLifeEmitter.emmiter = ContentBank.Inst.GetEmitter(typeof(BoomerangAmmoItem).Name); //TO: change to ID
            profile.CollusionUpdateList.Add(endLifeEmitter);

            profile.IsDestroyedOnCollision = false; //projectile is terminated on impact
            profile.IsEffectedByForce = true;
            profile.Draw = new ProjectileDrawRotateWithTime(0.1f, 0.1f, "boomerang", null);
            profile.MovementLogic = new MoveToTarget(ProjectileTargetType.AgentAncestor, 0.05f, 15);
            profile.VelocityInertia = 0.995f;
            profile.HitPointZeroEmiiterID = "BoomerangAmmoItem";
            profile.TimeOutEmitterID = "BoomerangAmmoItem";
            return profile;
        }
    }
}

