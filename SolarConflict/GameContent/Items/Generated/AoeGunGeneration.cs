using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Generated
{
    class AoeGunGeneration
    {
        public static IEmitter Make()
        {
            int skip = 2;
            int counter = 1;
            for (int i = 2; i < ScalingUtils.NumLevels; i += skip)
            {
                ContentBank.Inst.AddContent(MakeGun(i, counter));
                counter++;
            }
            return null;
        }

        public static Item MakeGun(int level, int counter)
        {
            WeaponData weaponData = new WeaponData("Aoe Ring Gun " + level.ToString());
            weaponData.ItemData.IconID = "AoeRingGunIcon";
            weaponData.ItemData.EquippedTextureId = "AoeRingGun";
            if (level > 0)
                weaponData.ItemData.SecounderyIconID = $"lvl{level}";
            weaponData.ItemData.EquippedTextureId = "turret1";
            weaponData.ItemData.Level = level;
            weaponData.ActiveTime = 1;
            weaponData.SoundEffectEmitterID = "sound_tensor_charge";
            weaponData.ItemData.Category = ItemCategory.EnergyConsumingWeapon | ItemCategory.Gun;

            string texture = "bigshockwave";

            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(level) * 2;

            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level);
            weaponData.Cooldown = 60;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 50;
            weaponData.ShotLifetime = 0;
            weaponData.ItemData.BuyPrice = 200;
            weaponData.EnergyCost = 1.5f * weaponData.ComputeEnergyActivationCost(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level));
            weaponData.ShotEmitter = MakeShot(level, weaponData.ComputeShotDamage(ScalingUtils.ScaleDamagePerFrame(level)), texture);
            Item item = WeaponQuickStart.Make(weaponData);
            item.ID = "AoeRingGun" + counter.ToString();
            
            return item;
        }

        public static ProjectileProfile MakeShot(int level, float damage, string texture)
        {
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = texture;
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatConst(80); //new InitFloatParentSize(1.1f, 2);
            projectileProfile.InitMaxLifetime = new InitFloatConst(Math.Max(level * 20, 20));
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(Color.White); //ScalingUtils.EffectColor(level));
            projectileProfile.CollisionSpec = new CollisionSpec(0, 0.001f);
            projectileProfile.IsDestroyedOnCollision = true;

            projectileProfile.ImpactEmitter = AoeShot(level, damage, texture);
            projectileProfile.TimeOutEmitter = projectileProfile.ImpactEmitter;
            projectileProfile.UpdateEmitter = Trail(level, texture);
            return projectileProfile;
        }

        public static ProjectileProfile AoeShot(int level,float damage, string texture)
        {
            float lifetime = Math.Max(level * 5, 25);
            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Collide1;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = texture;
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSizeID = "70";/// new InitFloatParentSize(1.1f, 0);
            projectileProfile.UpdateSize = new UpdateSizeGrow(15);
            projectileProfile.InitMaxLifetime = new InitFloatConst(lifetime);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(Color.White); //ScalingUtils.EffectColor(level));
        
            projectileProfile.CollisionSpec = new CollisionSpec(damage / lifetime, 0.001f) ;
            projectileProfile.VelocityInertia = 0.8f; //??
            projectileProfile.MovementLogic = new MoveToTarget(ProjectileTargetType.Parent, 0, 0); //??
            return projectileProfile;
        }

        public static IEmitter Trail(int level, string texture)
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.RotationSpeedType = ParamEmitter.EmitterRotationSpeed.Random;
            emitter.RotationSpeedRange = 80;
            emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            emitter.RotationRange = 360;

            ProjectileProfile projectileProfile = new ProjectileProfile();
            projectileProfile.CollisionType = CollisionType.Effects;
            projectileProfile.DrawType = DrawType.Additive;
            projectileProfile.ColorLogic = ColorUpdater.FadeInOut;
            projectileProfile.TextureID = texture;
            projectileProfile.CollisionWidth = projectileProfile.Sprite.Width - 20;
            projectileProfile.InitSize = new InitFloatParentSize(1.1f, 2);
            projectileProfile.InitMaxLifetime = new InitFloatConst(10);
            projectileProfile.IsDestroyedOnCollision = false;
            projectileProfile.InitColor = new InitColorConst(Color.White);// ScalingUtils.EffectColor(level));
            projectileProfile.CollisionSpec = new CollisionSpec(2, 0.001f);
            projectileProfile.VelocityInertia = 0.95f; //??  
            projectileProfile.IsDestroyedWhenParentDestroyed = true;
            emitter.Emitter = projectileProfile;
            return emitter;
        }
        
    }
}
