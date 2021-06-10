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
    class GuidedPlasmaGun
    {
        public static Item Make() //TODO: change to photon torpedo, needs energy
        {
            WeaponData weaponData = new WeaponData("Guided Plasma Gun");
            weaponData.ItemData.IconID = "GuidedPlasmaGunItem";
            weaponData.ItemData.EquippedTextureId = "VoidSiegegun";
            weaponData.ItemData.Level = 3;
            weaponData.ActiveTime = 1;
            weaponData.SoundEffectEmitterID = "sound_tensor_charge";

            ParamEmitter emitter = new ParamEmitter();
            emitter.Emitter = MakeMissile(ProjectileTargetType.None, 70);
            emitter.RotationType = ParamEmitter.EmitterRotation.RangeCenterd;
            emitter.RotationRange = MathHelper.TwoPi;
            emitter.MinNumberOfGameObjects = 6;

            weaponData.ShotEmitter = emitter;
            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level);
            weaponData.Cooldown = 60;            
            weaponData.ShotSpeed = 15;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level);
            weaponData.ItemData.SlotType = SlotType.Weapon;
            Item item = WeaponQuickStart.Make(weaponData);           
            return item;
        }

        public static ProjectileProfile MakeMissile(ProjectileTargetType targetType, float damage)
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.AggroRange = 600;
            projectileProfile.Light = Lights.SmallLight(new Color(40, 50, 200));
            //projectileProfile.AggroRange = 2000;
            projectileProfile.DrawType = DrawType.Additive;
            //projectileProfile.InitColor = new InitColorConst(colors[i]);
            // projectileProfile.ColorLogic = ColorUpdater.FadeOutSlow;
            projectileProfile.TextureID = "add4";
            projectileProfile.Draw = new ProjectileDrawRotateWithTime(-0.11f, 0.1f, "add4", "add4");
            projectileProfile.InitSizeID = "20";
            projectileProfile.UpdateSize = null;
            projectileProfile.MovementLogic = new MoveForward(0.7f, 25);
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
            trailEffect.SizeBase = 30;
            projectileProfile.UpdateEmitter = trailEffect;
            projectileProfile.UpdateEmitterCooldownTime = 2;
            projectileProfile.InitParam = new InitFloatRotation();
            //if (isGuided)
            var rotationUpdate = new UpdateRotationOffsetHoming(rotationSpeed:1.5f, targetType: targetType);
            rotationUpdate.TimeToStart = 0;
            rotationUpdate.NoTargetRotation = UpdateRotationOffsetHoming.NoTargetRotationType.Parent;
            rotationUpdate.SecounderyTargetType = ProjectileTargetType.None;
            projectileProfile.RotationLogic = rotationUpdate;
            projectileProfile.ImpactEmitterID = typeof(EmitterImpactFx1).Name;
            projectileProfile.TimeOutEmitterID = typeof(EmitterImpactFx1).Name;
            projectileProfile.CollisionSpec = new CollisionSpec(damage, 0.5f);
            projectileProfile.CollisionSpec.IsDamaging = true;
            projectileProfile.IsDestroyedOnCollision = true;
            projectileProfile.IsEffectedByForce = false;
            projectileProfile.CollisionType = CollisionType.Collide1;
            return projectileProfile;
        }


    }
}

