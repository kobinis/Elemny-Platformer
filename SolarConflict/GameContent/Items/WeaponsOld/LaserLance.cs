//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.Generation;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.Weapons {
//    /// <remarks>TEMP class. Lasers should be scaled via .csvs and Tweaks, we don't need a separate class for each, and we definitely don't
//    /// need three</remarks>
//    class LaserLance {
//        public static Item Make() {
//            var level = 3;
//            // Beam segment            
//            var bitProfile = new ProjectileProfile();
//            bitProfile.ID = "LaserLance_MainEmitter_BeamEmitter_BeamBit";
//            bitProfile.DrawType = DrawType.Additive;
//            bitProfile.InitColor = new InitColorConst(new Color(200, 50, 50));
//            bitProfile.ColorLogic = ColorUpdater.FadeOutSlow;
//            bitProfile.TextureID = "add10";
//            bitProfile.CollisionWidth = bitProfile.Sprite.Width - 10;
//            bitProfile.InitSizeID = "20";
//            bitProfile.UpdateSize = null;
//            bitProfile.InitMaxLifetime = new InitFloatConst(1);
//            bitProfile.Mass = 0.1f;
//            bitProfile.ImpactEmitterID = typeof(LaserHitFx).Name;
//            bitProfile.CollisionSpec = new CollisionSpec(1.2f, 0f);
//            bitProfile.CollisionSpec.AddEntry(MeterType.MiningDamage, 2, ImpactType.Max);
//            bitProfile.IsDestroyedOnCollision = true;
//            bitProfile.IsEffectedByForce = false;

//            // Beam effect emitter
//            var beamEmitter = new ParamEmitter();
//            beamEmitter.ID = "LaserLance_MainEmitter_BeamEmitter";
//            beamEmitter.Emitter = bitProfile;
//            beamEmitter.PosAngleType = ParamEmitter.EmitterPosAngle.Const;
//            beamEmitter.PosRadType = ParamEmitter.EmitterPosRad.Range;
//            beamEmitter.PosRadMin = 0;
//            beamEmitter.PosRadRange = 650;

//            beamEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.PosAngle;

//            beamEmitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Const;
//            beamEmitter.VelocityMagMin = 0;

//            beamEmitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Const;
//            beamEmitter.RotationSpeedRange = 0;

//            beamEmitter.MinNumberOfGameObjects = 50;

//            beamEmitter.LifetimeType = ParamEmitter.EmitterLifetime.Const;
//            beamEmitter.LifetimeMin = 1;

//            // System
//            var system = new EmitterCallerSystem();
//            system.Emitter = beamEmitter;
            

//            system.ActivationCheck.AddCost(MeterType.Energy, ScalingUtils.ScaleEnergyCostPerFrame(level) * 2f);
//            system.refVelocityMult = 1;
//            system.velocity = Vector2.Zero;
//            system.SecondaryEmitterID = typeof(LaserFlashFx).Name;
//            system.secondaryVelocityMult = 0.1f;

//            // Item
//            ItemProfile profile = ItemQuickStart.Profile("Laser Lance", "Short-range laser weapon, effective vs large ships", 0,
//                "AttackDrone", null);
//            profile.Level = level;
//            profile.SlotType = SlotType.Weapon;            
//            profile.BuyPrice = 10000;
//            profile.SellPrice = 7000;
//            profile.BreaksCloaking = true;
//            Item result = new Item(profile);

//            result.System = system;
         
//            return result;
//        }
//    }
//}
