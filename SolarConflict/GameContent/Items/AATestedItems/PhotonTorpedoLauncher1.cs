using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitFloat;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class PhotonTorpedoLauncher1
    {
        public static Item Make() //TODO: change to photon torpedo, needs energy
        {
            WeaponData weaponData = new WeaponData("Photon Torpedo Launcher");
            ParamEmitter paramEmitter = new ParamEmitter();
            paramEmitter.Emitter = MakeMissile(ProjectileTargetType.Enemy, 20, AoeDamage1.Make());
            paramEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.RangeCenterd;
            paramEmitter.RotationRange = 180;
            paramEmitter.VelocityMagMin = 3;
            paramEmitter.MinNumberOfGameObjects = 1;

            //weaponData.ItemData.Description = "Lunches Photon Torpedoes";
            weaponData.ItemData.IconID = "PhotonTorpedoLauncherItem";
            weaponData.ItemData.EquippedTextureId = "turret1";
            weaponData.ItemData.Level = 5;
            weaponData.SoundEffectEmitterID = "sound_pulse";
            weaponData.ActiveTime = 1;
            weaponData.ShotEmitter = paramEmitter;
            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level);            
            weaponData.Cooldown = 30;
            weaponData.MidCooldownTime = 4;
            weaponData.ActiveTime = 20;
            //weaponData.IsTurreted = true;
            weaponData.ShotSpeed = 15;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level);
            weaponData.Range = 2500; 
            Item item = WeaponQuickStart.Make(weaponData);
            return item;
        }


        public static ProjectileProfile MakeMissile(ProjectileTargetType targetType, float damage, IEmitter emitter)
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.AggroRange = 250;
            projectileProfile.Light = Lights.SmallLight(new Color(40,50,200));
            //projectileProfile.AggroRange = 2000;
            projectileProfile.DrawType = DrawType.Additive;
            //projectileProfile.InitColor = new InitColorConst(colors[i]);
            // projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;
            projectileProfile.Draw = new ProjectileDrawRotateWithTime(0.1f, 0.12f, "add2c");
            projectileProfile.TextureID = "add8";
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 10;
            projectileProfile.InitSizeID = "15";
            projectileProfile.UpdateSize = null;
            projectileProfile.MovementLogic = new MoveForward(0.8f, 35);
            projectileProfile.InitMaxLifetime = new InitFloatConst(180);
            projectileProfile.Mass = 0.1f;
            projectileProfile.VelocityInertia = 0.99f;
            ParamEmitter trailEffect = new ParamEmitter();
            trailEffect.EmitterID = typeof(EmitterFxSmoke).Name;
            trailEffect.RefVelocityMult = 0.8f;
            trailEffect.VelocityAngleBase = MathHelper.Pi;
            trailEffect.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Const;
            trailEffect.VelocityAngleBase = 5f;
            trailEffect.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Const;
            trailEffect.RotationType = ParamEmitter.EmitterRotation.Random;
            trailEffect.RotationRange = 360;
            trailEffect.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            trailEffect.RotationSpeedRange = 10;
            trailEffect.SizeType = ParamEmitter.InitSizeType.Const;
            trailEffect.SizeBase = 1;
            projectileProfile.UpdateEmitter = trailEffect;
            projectileProfile.UpdateEmitterCooldownTime = 1;
            //projectileProfile.UpdateEmitterID = "GenericTrail";
            projectileProfile.InitParam = new InitFloatRotation();
            //if (isGuided)
            var rotationUpdate = new UpdateRotationOffsetHoming(rotationSpeed:0.3f, targetType: targetType);
            rotationUpdate.TimeToStart = 0;
            rotationUpdate.NoTargetRotation = UpdateRotationOffsetHoming.NoTargetRotationType.Param;
            rotationUpdate.SecounderyTargetType = ProjectileTargetType.None;
            projectileProfile.RotationLogic = rotationUpdate;
            projectileProfile.ImpactEmitter = emitter;// "FireFx";
            projectileProfile.TimeOutEmitter = emitter;
            projectileProfile.CollisionSpec = new CollisionSpec(damage, 0.5f);
            projectileProfile.CollisionSpec.IsDamaging = true;
            projectileProfile.IsDestroyedOnCollision = true;
            projectileProfile.IsEffectedByForce = false;
            projectileProfile.CollisionType = CollisionType.Collide1;
            return projectileProfile;
        }

    }
}
