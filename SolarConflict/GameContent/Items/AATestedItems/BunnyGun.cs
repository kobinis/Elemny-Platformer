//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitFloat;
//using SolarConflict.GameContent.Emitters.Effects;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using SolarConflict.NewContent.Emitters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.AATestedItems
//{
//    class BunnyGun
//    {
//        public static Item Make()
//        {
//            WeaponData weaponData = new WeaponData("Bunny Gun");
//            weaponData.ItemData.IconID = "bunny2";
//            weaponData.ItemData.EquippedTextureId = "turret1";
//            weaponData.ItemData.Level = 4;

//            weaponData.Cooldown = 15;
//            weaponData.ActiveTime = 1;
//            float damage = (float)Math.Round(ScalingUtils.ScaleDamagePerFrame(weaponData.ItemData.Level) * 60);
//            weaponData.ShotEmitter = MakeShot(damage, 1);
//            weaponData.ShotLifetime = (int)(60 * 10f);
//            weaponData.EnergyCost = (float)Math.Round(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level) * weaponData.Cooldown / 60f * 2f); ;
//            //2 weaponData.IsTurreted = false;
//            weaponData.ShotSpeed = 1;
//            weaponData.ItemData.BuyPrice = 200;
//            weaponData.KickbackForce = 0.1f;
//            weaponData.EffectEmitterID = "EmitterFxSmoke";
//            //TODO: speed mult = 0;
//            Item item = WeaponQuickStart.Make(weaponData);
//            return item;
//        }


//        public static ProjectileProfile MakeShot(float damage, float force)
//        {
//            ProjectileProfile projectileProfile = new ProjectileProfile();
//            projectileProfile.AggroRange = 250;
//            projectileProfile.Light = Lights.SmallLight(new Color(40, 50, 200));
//            //projectileProfile.AggroRange = 2000;
//            projectileProfile.DrawType = DrawType.Alpha;
//            //projectileProfile.InitColor = new InitColorConst(colors[i]);
//            //projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
//            projectileProfile.TextureID = "bunnyShot";
//            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 10;
//            projectileProfile.InitSizeID = "70";
//            projectileProfile.UpdateSize = null;
//            projectileProfile.MovementLogic = new MoveForward(0.8f, 5);
//            projectileProfile.InitMaxLifetime = new InitFloatConst(980);
//            projectileProfile.Mass = 0.1f;
//            projectileProfile.VelocityInertia = 0.99f;
//            ParamEmitter trailEffect = new ParamEmitter();
//            trailEffect.EmitterID = typeof(EmitterFxSmoke).Name;
//            trailEffect.RefVelocityMult = 0.8f;
//            trailEffect.VelocityAngleBase = MathHelper.Pi;
//            trailEffect.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Const;
//            trailEffect.VelocityAngleBase = 5f;
//            trailEffect.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Const;
//            trailEffect.RotationType = ParamEmitter.EmitterRotation.Random;
//            trailEffect.RotationRange = 360;
//            trailEffect.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
//            trailEffect.RotationSpeedRange = 10;
//            trailEffect.SizeType = ParamEmitter.InitSizeType.Const;
//            trailEffect.SizeBase = 1;
//            projectileProfile.UpdateEmitter = trailEffect;
//            projectileProfile.UpdateEmitterCooldownTime = 2;
//            projectileProfile.InitParam = new InitFloatRotation();
//            //if (isGuided)
//            var rotationUpdate = new UpdateRotationOffsetHoming(rotationSpeed: 0.1f, targetType: ProjectileTargetType.Enemy);
//            rotationUpdate.TimeToStart = 0;
//            rotationUpdate.NoTargetRotation = UpdateRotationOffsetHoming.NoTargetRotationType.Param;
//            rotationUpdate.SecounderyTargetType = ProjectileTargetType.None;
//            projectileProfile.RotationLogic = rotationUpdate;
//            projectileProfile.ImpactEmitterID = "BloodSplashFx1"; //add bunny gibs
//            projectileProfile.CollisionSpec = new CollisionSpec(damage, 0.5f);
//            projectileProfile.CollisionSpec.IsDamaging = true;
//            projectileProfile.IsDestroyedOnCollision = true;
//            projectileProfile.IsEffectedByForce = false;
//            projectileProfile.CollisionType = CollisionType.Collide1;
//            return projectileProfile;
//        }
//    }
//}
