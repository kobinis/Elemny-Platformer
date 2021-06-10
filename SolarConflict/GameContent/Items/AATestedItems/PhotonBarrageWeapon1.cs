using Microsoft.Xna.Framework;
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
    class PhotonBarrageWeapon1
    {
        public static Item Make() //TODO: change to photon torpedo, needs energy
        {
            WeaponData weaponData = new WeaponData("Photon Barrage");
            //weaponData.ItemData.Description = "Lunches Photon Torpedoes";
            weaponData.ItemData.IconID = "PhotonBarrageItem";
            weaponData.ItemData.EquippedTextureId = null;
            weaponData.ItemData.Level = 7;            
            weaponData.ActiveTime = 90;

            ParamEmitter paramEmitter = new ParamEmitter();
            paramEmitter.Emitter = MakeMissile(ProjectileTargetType.Enemy, 50, AoeDamage1.Make());

            paramEmitter.RotationType = ParamEmitter.EmitterRotation.Random;
            paramEmitter.RotationRange = 360;            
            paramEmitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Range;
            paramEmitter.VelocityAngleRange = 360;
            paramEmitter.VelocityMagMin = 3;
            paramEmitter.MinNumberOfGameObjects = 1;
            weaponData.SoundEffectEmitterID = null;

            weaponData.ActivationEmitterID = "sound_tensor_charge";
            weaponData.ShotEmitter = paramEmitter;
            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level) ;
            weaponData.Cooldown = 60*10;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 15;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level);
            Item item = WeaponQuickStart.Make(weaponData);
            item.Profile.SlotType |= SlotType.Turret;
            return item;
        }


        public static ProjectileProfile MakeMissile(ProjectileTargetType targetType, float damage, IEmitter emitter)
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.Light = Lights.SmallLight(new Color(200, 100, 50));
            //projectileProfile.AggroRange = 2000;
            projectileProfile.DrawType = DrawType.Additive;
            //projectileProfile.InitColor = new InitColorConst(colors[i]);
            // projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;
            projectileProfile.AggroRange = 1000;
            projectileProfile.Draw = new ProjectileDrawRotateWithTime(0.1f, 0.12f, "add2");
            projectileProfile.TextureID = "add2";
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
            projectileProfile.UpdateEmitterCooldownTime = 2;
            projectileProfile.InitParam = new InitFloatRandom(-MathHelper.Pi, MathHelper.TwoPi);
            //if (isGuided)
            var rotationUpdate = new UpdateRotationOffsetHoming(targetType: targetType);
            rotationUpdate.TimeToStart = 10;
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
