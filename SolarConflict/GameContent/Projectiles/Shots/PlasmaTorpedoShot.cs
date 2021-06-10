//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.Framework.Utils;
//using SolarConflict.NewContent.Emitters;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using XnaUtils.Graphics;

//namespace SolarConflict.GameContent.Projectiles.Shots {
//    class PlasmaTorpedoShot {
//        public static ProjectileProfile Make() {            
//            var durations = new int[] { Utility.Frames(0.5f), Utility.Frames(3f) };
//            var damageRange = new float[] { 30f, 600f };
//            var sizes = new int[] { 15, 135 };
//            var deltaSize = ((float)sizes[1] - sizes[0]) / durations[1];


//            Func<ProcessContext, ProcessStatus> phaseTwoCallback = (context) => {
//                var projectile = (context.Parent as Projectile);

//                if (!projectile.IsActive)
//                    return ProcessStatus.Failed;

//                // Damage and size increase over time. We do this here instead of via UpdateSize() etc in order to avoid rounding errors,
//                // which can be significant with ints this small interpolated over so many frames via a fixed delta

//                // First, give the projectile a clone of its profile's collision spec; we'll be modifying it
//                if (projectile.CollisionInfo == projectile.profile.CollisionSpec && projectile.CollisionInfo != null)
//                    projectile.CollisionInfo = (CollisionInfo)projectile.CollisionInfo.GetDeepCopy();

//                // Increase damage with time                
//                var impactList = projectile.CollisionInfo.ImpactList;

//                Debug.Assert(impactList.Where(entry => entry.meterType == MeterType.Damage).Count() == 1);

//                var damageSpec = impactList.First(entry => entry.meterType == MeterType.Damage);

//                damageSpec.amount = damageRange[0] + (damageRange[1] - damageRange[0]) * projectile.Lifetime / durations[1];

//                impactList = impactList.Where(entry => entry.meterType != MeterType.Damage).ToList();
//                impactList.Add(damageSpec);
//                projectile.CollisionInfo.ImpactList = impactList;

//                return ProcessStatus.InProgress;
//            };


//            var phaseOneProcess = new ProcessChain(new Wait(durations[0]), new Destroy(), new Emit("PlasmaTorpedoShotPhaseTwo", 1));
//            var phaseTwoProcess = new ProcessChain(new Generic(phaseTwoCallback, "PlasmaTorpedoShotPhaseTwoProcess"));

            
//            // TODO: visuals
//            // Phase one
//            var projectilePhaseOne = new ComplexProjectileProfile();
//            projectilePhaseOne.Process = phaseOneProcess;
//            projectilePhaseOne.ID = "PlasmaTorpedoShot";
//            projectilePhaseOne.DrawType = DrawType.Additive;
//            projectilePhaseOne.InitColor = new InitColorConst(Color.Green);
//            projectilePhaseOne.ColorLogic = ColorUpdater.FadeOutSlow;
//            projectilePhaseOne.TextureID = "disrupter";
//            projectilePhaseOne.CollisionWidth = projectilePhaseOne.Sprite.Width - 10;
//            projectilePhaseOne.InitSizeID = sizes[0].ToString();
//            projectilePhaseOne.MovementLogic = new MoveForward(0.06f, 3f);
//            projectilePhaseOne.Mass = 0.1f;
//            projectilePhaseOne.CollisionSpec = new CollisionInfo(1f, 0.5f);
//            projectilePhaseOne.CollisionSpec.IsDamaging = true;
//            projectilePhaseOne.IsDestroyedOnCollision = true;
//            projectilePhaseOne.IsEffectedByForce = false;            

//            // Phase two
//            var projectilePhaseTwo = new ComplexProjectileProfile();
//            projectilePhaseTwo.Process = phaseTwoProcess;
//            projectilePhaseTwo.ID = "PlasmaTorpedoShotPhaseTwo";
//            projectilePhaseTwo.DrawType = DrawType.Additive;
//            projectilePhaseTwo.InitColor = new InitColorConst(Color.Green);
//            projectilePhaseTwo.ColorLogic = ColorUpdater.FadeOutSlow;
//            projectilePhaseTwo.TextureID = "add3";
//            projectilePhaseTwo.CollisionWidth = projectilePhaseTwo.Sprite.Width - 10;
//            projectilePhaseTwo.InitSizeID = sizes[0].ToString();
//            projectilePhaseTwo.UpdateSize = new UpdateSizeGrow(deltaSize);
//            projectilePhaseTwo.MovementLogic = new MoveForward(0.11f, 3f);
//            // TODO: maybe modify per-projectile movement logic via the process, too, make the rate of acceleration increase over time (but don't improve the turn rate)
//            projectilePhaseTwo.InitMaxLifetime = new InitFloatConst(durations[1]);
//            projectilePhaseTwo.Mass = 0.1f;

//            // Phase two trail
//            // TODO: maybe make the trail elements ComplexProjectiles, scale their damage (on spawn) as we do the torpedo's
//            projectilePhaseTwo.UpdateEmitterID = typeof(PlasmaTrail).Name;
//            projectilePhaseTwo.UpdateEmitterCooldownTime = 1;

//            projectilePhaseTwo.RotationLogic = new UpdateRotationHoming();
//            projectilePhaseTwo.ImpactEmitterID = typeof(SmalFlameFx).Name;
//            projectilePhaseTwo.TimeOutEmitterID = typeof(SmokeExplosionFx).Name; // TODO: replace
//            projectilePhaseTwo.CollisionSpec = new CollisionInfo(30, 0.5f);
//            projectilePhaseTwo.CollisionSpec.IsDamaging = true;
//            projectilePhaseTwo.IsDestroyedOnCollision = true;
//            projectilePhaseTwo.IsEffectedByForce = false;

//           // projectilePhaseTwo.ApplyTags(Color.Green.ToVector3(), "energy", "medium", "bright");

//            ContentBank.Inst.AddContent(projectilePhaseTwo);

//            return projectilePhaseOne;
//        }
//    }
//}
