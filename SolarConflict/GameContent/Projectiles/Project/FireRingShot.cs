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

//namespace SolarConflict.GameContent.Projectiles.Shots
//{
//    class FireRingShot
//    {
//        public static ProjectileProfile Make()
//        {
//            return MakeWithTweaks(typeof(FireRingShot).Name);
//        }

//        public static ProjectileProfile MakeWithTweaks(string outputID)
//        {
//            var damageRange = new float[] { 2f, 80f };
//            var rampUpTime = Utility.Frames(0.67f);
//            var additionalDuration = Utility.Frames(1f);

//            Func<ProcessContext, ProcessStatus> callback = (context) =>
//            {
//                // Grow stronger over time (so burst isn't obscenely powerful at point blank)
//                var projectile = (context.Parent as Projectile);

//                if (!projectile.IsActive)
//                    return ProcessStatus.Failed;


//                // First, give the projectile a clone of its profile's collision spec; we'll be modifying it
//                if (projectile.CollisionInfo == projectile.profile.CollisionSpec && projectile.CollisionInfo != null)
//                    projectile.CollisionInfo = (CollisionInfo)projectile.CollisionInfo.GetDeepCopy();

//                // Increase damage with time                
//                var impactList = projectile.CollisionInfo.ImpactList;

//                Debug.Assert(impactList.Where(entry => entry.meterType == MeterType.Damage).Count() == 1);

//                var damageSpec = impactList.First(entry => entry.meterType == MeterType.Damage);

//                var ratio = Math.Min(1f, ((float)projectile.Lifetime) / rampUpTime);

//                damageSpec.amount = damageRange[0] + (damageRange[1] - damageRange[0]) * ratio;

//                impactList = impactList.Where(entry => entry.meterType != MeterType.Damage).ToList();
//                impactList.Add(damageSpec);
//                projectile.CollisionInfo.ImpactList = impactList;

//                return ratio >= 0.9999f ? ProcessStatus.Done : ProcessStatus.InProgress;
//            };

//            var profile = new ComplexProjectileProfile();
//            profile.ID = outputID;
//            profile.Process = new Generic(callback, outputID + "_Process");
//            profile.DrawType = DrawType.Additive;
//            profile.ColorLogic = ColorUpdater.FadeOutSlow;
//            profile.TextureID = "add2";
//            profile.CollisionWidth = profile.Sprite.Width + 20;
//            profile.InitSize = new InitFloatConst(60 * 1);
//            profile.UpdateSize = null;
//            profile.InitMaxLifetime = new InitFloatConst(100);
//            profile.Mass = 0.1f;
//            profile.UpdateEmitterCooldownTime = 3;
//            profile.ImpactEmitterID = typeof(DamageAoe).Name;
//            profile.CollisionSpec = new CollisionInfo(50, 0.5f);
//            profile.IsDestroyedOnCollision = true;
//            profile.IsEffectedByForce = false;

//            return profile;
//        }
//    }
//}
