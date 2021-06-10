//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SolarConflict.GameContent.Projectiles;
//using SolarConflict.GameContent.Emitters;
//using Microsoft.Xna.Framework;
//using SolarConflict.NewContent.Emitters;
//using XnaUtils.Graphics;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.NewContent.Projectiles;

//namespace SolarConflict.GameContent.Items {
//    class RiftBeam {

//        /// <remarks>TODO: incomplete, needs visual polish, probably contains some redundant copypasted stuff.
//        /// 
//        /// TODO: * Make the attraction much stronger, make nerf the damage, make the visible area deal more damage
//        /// * Test a bunch, ensure a sufficiently large rift attracts ships/projectiles in interesting ways, ensure
//        /// playing with the shape of the rift has noticable effects. Tweak rift lifetime if need be
//        /// * Ensure energy cost is prohibitive enough
//        /// </remarks>
        
//        public static Item Make() {
//            var beamLength = 600f;
//            var segmentSize = 10;
//            var numSegments = (int)Math.Round(beamLength / segmentSize);

//            // Rift
//            var riftProfile = new ProjectileProfile();
//            riftProfile.ID = "RiftBeam_MainEmitter_RiftEmitter_Rift";

//            float sizeMultiplier = 6f;

//            riftProfile.DrawType = DrawType.Additive;
//            riftProfile.ColorLogic = ColorUpdater.FadeOutSlow;
//            riftProfile.TextureID = "add14"; //add14
//            riftProfile.CollisionWidth = riftProfile.Sprite.Width * sizeMultiplier;
//            riftProfile.InitSizeID = "10";
//            riftProfile.UpdateSize = new UpdateSizeGrow(1, 1.015f, 550 * sizeMultiplier);
//            riftProfile.InitMaxLifetimeID = "600"; //1/60 sec
//            riftProfile.Mass = 0.1f;
//            //profile.ImpactEmitterId = typeof(EmitterImpactFx1).Name;
//            //profile.InitHitPointsId = "1000";
//            riftProfile.IsDestroyedOnCollision = false;
//            riftProfile.IsEffectedByForce = false;
//            //profile.UpdateMovement = new MoveToTarget(ProjectileTargetType.Enemy, 0.8f, 15); 
//            //profile.UpdateRotation = new UpdateRotationForward();
//            riftProfile.CollisionSpec = new CollisionSpec(1.5f, -120f);
//            riftProfile.CollisionSpec.ForceType = ForceType.Gravity;
//            riftProfile.CollisionSpec.Flags |= CollisionSpecFlags.AffectsAllies; ;
//            riftProfile.VelocityInertia = 0.85f;            

//            // Rift emitter (just spawns the rift with an offset). Aims for the end of the beam, but isn't actually a child
//            // of the beam. Beam and rift are siblings, beam is decorative.
//            var riftEmitter = new ParamEmitter();
//            riftEmitter.ID = "RiftBeam_MainEmitter_RiftEmitter";

//            riftEmitter.Emitter = riftProfile;

//            riftEmitter.PosRadType = ParamEmitter.EmitterPosRad.Const;
//            riftEmitter.PosRadMin = beamLength;

//            // Beam effect segment            
//            var bitProfile = new ProjectileProfile();
//            bitProfile.ID = "RiftBeam_MainEmitter_BeamEmitter_Bit";
//            bitProfile.DrawType = DrawType.Additive;
//            bitProfile.InitColor = new InitColorConst(new Color(44, 117, 255));
//            bitProfile.ColorLogic = ColorUpdater.FadeOutSlow;
//            bitProfile.TextureID = "add10";            
//            bitProfile.CollisionWidth = bitProfile.Sprite.Width - 10;
//            bitProfile.InitSizeID = segmentSize.ToString();
//            bitProfile.UpdateSize = null;
//            bitProfile.InitMaxLifetime = new InitFloatConst(1);
//            bitProfile.Mass = 0.1f;
//            //bitProfile.ImpactEmitterID = typeof(LaserHitFx).Name;            
//            bitProfile.IsDestroyedOnCollision = false;
//            bitProfile.IsEffectedByForce = false;

//            // Beam effect emitter
//            var beamEmitter = new ParamEmitter();
//            beamEmitter.ID = "Riftbeam_MainEmitter_BeamEmitter";
//            beamEmitter.Emitter = bitProfile;
//            beamEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
//            beamEmitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
//            beamEmitter.PosRadMin = 0;
//            beamEmitter.PosRadRange = beamLength;

//            beamEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

//            beamEmitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
//            beamEmitter.VelocityMagMin = 0;

//            beamEmitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Const;
//            beamEmitter.RotationSpeedRange = 0;

//            beamEmitter.MinNumberOfGameObjects = numSegments;

//            beamEmitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
//            beamEmitter.LifetimeMin = 1;

//            // Main emitter
//            var mainEmitter = new GroupEmitter();
//            mainEmitter.ID = "RiftBeam_MainEmitter";
//            mainEmitter.AddEmitter(beamEmitter);
//            mainEmitter.AddEmitter(riftEmitter);


//            // System
//            var system = new EmitterCallerSystem();
//            system.Emitter = mainEmitter;
//            system.ActivationCheck.AddCost(MeterType.Energy, 2);
//            system.refVelocityMult = 1;
//            system.velocity = Vector2.Zero;
//            system.SecondaryEmitterID = typeof(LaserFlashFx).Name;
//            system.secondaryVelocityMult = 0.1f;

//            // Item
//            var profile = ItemQuickStart.Profile("Rift Beam", "Creates gravitational rifts that attract objects", 11,
//                "replace", null);
//            profile.SlotType = SlotType.Weapon;
//            profile.BreaksCloaking = true;
//            profile.Level = 11;

//            var result = new Item(profile);
//            result.System = system;

//            return result;
//        }        
//    }
//}
 