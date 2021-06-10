//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.Framework.Process;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Projectiles;
//using System;
//using XnaUtils.Graphics;

///// <summary>A big, glowy explosion.</summary>
//namespace SolarConflict.GameContent.Emitters.Effects {
//    class StackpoleFx {
//        static int[] _durations = new int[] { Utility.Frames(0.3f), Utility.Frames(1f), Utility.Frames(0.3f) };

//        public static ProjectileProfile Make() {
//            var light = Lights.BlindingLight(new Vector3(1f, 0.388f, 0.15f)) as PointLight;
//            var calculator = (light.IntensityCalculator as IntensityCalculators.InverseMononomial);
//            var maxIntensity = calculator.BaseIntensity;
//            calculator.BaseIntensity = 0f;

//            var rampUpIncrement = maxIntensity / _durations[0];
//            var rampDownIncrement = maxIntensity / _durations[2];

//            // Ramp up
//            Func<ProcessContext, ProcessStatus> RampUp = (context) => {
//                var projectile = (context.Parent as Projectile);
                
//                if (!projectile.IsActive)
//                    return ProcessStatus.Failed;

//                Utility.Assert(projectile.Lights.Count == 1, "More lights than the Stackpole Effect knows how to handle");
//                // ^ This isn't terribly robust. Maybe lights should have a (non-unique, cloneable) ID

//                var lightCalculator = ((projectile.Lights[0] as PointLight).IntensityCalculator as IntensityCalculators.InverseMononomial);
//                lightCalculator.BaseIntensity += rampUpIncrement;

//                if (lightCalculator.BaseIntensity >= maxIntensity)
//                    return ProcessStatus.Done;

//                return ProcessStatus.InProgress;
//            };

//            // Ramp down
//            Func<ProcessContext, ProcessStatus> RampDown = (context) => {
//                var projectile = (context.Parent as Projectile);

//                if (!projectile.IsActive)
//                    return ProcessStatus.Failed;

//                Utility.Assert(projectile.Lights.Count == 1, "More lights than the Stackpole Effect knows how to handle");

//                var lightCalculator = ((projectile.Lights[0] as PointLight).IntensityCalculator as IntensityCalculators.InverseMononomial);
//                lightCalculator.BaseIntensity -= rampDownIncrement;

//                if (lightCalculator.BaseIntensity <= 0f)
//                    return ProcessStatus.Done;

//                return ProcessStatus.InProgress;
//            };

//            MakeSecondaryEmitters();

//            var result = new ComplexProjectileProfile();
//            result.Process = new ProcessChain(new Emit("StackpoleFxExplosionFx"), new Generic(RampUp, "StackpoleFxRampUp"), new Wait(_durations[1]), new Generic(RampDown, "StackpoleFxRampDown"));
            
//            result.Lights = new ILight[] { light };
//            result.ID = "StackpoleFx";
//            result.DrawType = DrawType.Additive;

//            result.CollisionType = CollisionType.Effects;            

//            return result;
//        }

//        static void MakeSecondaryEmitters() {
//            // Mostly copypasted, timing changed to match the light emission            
//            ParamEmitter fireExplosionEmitter = new ParamEmitter();
//            fireExplosionEmitter.ID = "StackpoleFxFireExplosionFx";
//            fireExplosionEmitter.EmitterId = typeof(ExplosionParticleFx).Name;

//            fireExplosionEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
//            fireExplosionEmitter.VelocityAngleRange = 360;

//            fireExplosionEmitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
//            fireExplosionEmitter.VelocityMagMin = 0.1f;
//            fireExplosionEmitter.VelocityMagRange = 1f;

//            fireExplosionEmitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
//            fireExplosionEmitter.RotationSpeedRange = MathHelper.ToDegrees(0.1f);

//            fireExplosionEmitter.RotationType = ParamEmitter.EmitterRotation.Random;
//            fireExplosionEmitter.RotationRange = 360;

//            fireExplosionEmitter.MinNumberOfGameObjects = 20;

//            fireExplosionEmitter.LifetimeType = ParamEmitter.EmitterLifetime.Random;
//            fireExplosionEmitter.LifetimeMin = _durations[0] + _durations[1] + _durations[2];
//            fireExplosionEmitter.LifetimeRange = 5;

//            ContentBank.Inst.AddContent(fireExplosionEmitter);

//            // Shockwave
//            // Also copypasted
//            ProjectileProfile shockwave = new ProjectileProfile();
//            shockwave.ID = "StackpoleFxShockwaveFx";
//            shockwave.CollisionType = CollisionType.Effects;
//            shockwave.DrawType = DrawType.Additive;
//            shockwave.ColorLogic = ColorUpdater.FadeInOut;
//            shockwave.TextureID = "shockwave2";
//            shockwave.ScaleMult = 1f / (float)shockwave.Sprite.Width;
//            shockwave.InitSize = new InitFloatConst(15);
//            shockwave.UpdateSize = new UpdateSizeGrowMult(1.05f);
//            shockwave.InitMaxLifetime = new InitFloatConst(_durations[0] + _durations[1] + _durations[2]);

//            ContentBank.Inst.AddContent(shockwave);

//            // Explosion
//            // TEMP: pretty much copypasted from FullExplosionFx1
//            GroupEmitter explosionEmitter = new GroupEmitter();
//            explosionEmitter.ID = "StackpoleFxExplosionFx";
//            explosionEmitter.RefVelocityMult = 0.1f;
//            explosionEmitter.AddEmitter("StackpoleFxFireExplosionFx");
//            explosionEmitter.AddEmitter("SmokeExplosionFx");
//            explosionEmitter.AddEmitter("StackpoleFxShockwaveFx");
//            explosionEmitter.AddEmitter("EmitterDebris1");
//            explosionEmitter.AddEmitter("ProjShipwreck1");
//            explosionEmitter.AddEmitter(new SoundEmitter(SoundEngine.GetSoundEffect("exp2"), 1)); //change                

//            ContentBank.Inst.AddContent(explosionEmitter);
//        }
//    }
//}
