
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.ContentGeneration.Items;
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
    class ShotgunGeneration
    {
        public static IEmitter Make()
        {
            int skip = 2;
            int counter = 1;         
            for (int i = 1; i < ScalingUtils.NumLevels; i+= skip)
            {
                ContentBank.Inst.AddContent(MakeShotgunItem(i, counter));
                counter++;
            }
            return null;            
        }

        public static Item MakeShotgunItem(int level, int counter)
        {
            var cooldown = Utility.Frames(0.5f);
            var damagePerCluster = 2f * ScalingUtils.ScaleDamagePerFrame(level) * (cooldown + 1); // 2 times base DPS 

            WeaponData weaponData = new WeaponData($"Kinetic Shotgun {counter}");
            //weaponData.ItemData.Description = "Shoots a number of small pellets \nthat deal damage according to the impact speed.";
            weaponData.ItemData.IconID = "ShotgunIcon";
            if (level > 0)
                weaponData.ItemData.SecounderyIconID = $"lvl{level}";
            weaponData.ItemData.EquippedTextureId = "Shotgun";
            weaponData.ItemData.Category = ItemCategory.EnergyConsumingWeapon | ItemCategory.Shotgun | ItemCategory.Gun;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(level);
            weaponData.ItemData.Level = level;

            weaponData.ShotEmitter = MakeShotCluster(level, damagePerCluster);
            weaponData.ShotSpeed = 20 + 3 * 5; //5

            weaponData.Cooldown = cooldown;
            //weaponData.IsTurreted = false;
            weaponData.KickbackForce = Math.Max( 1 - level*0.1f, 0.3f);
            weaponData.EnergyCost = ScalingUtils.ScaleEnergyCostPerFrame(level);

            weaponData.EffectEmitterID = typeof(GunFlashFx).Name;
            weaponData.EffectColor = new Color(200, 200, 255);
            weaponData.SoundEffectEmitterID = "sound_shotgun";
            weaponData.EnergyCost = 1f * weaponData.ComputeEnergyActivationCost(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level));
            weaponData.Range = (int)( weaponData.ShotSpeed * 60);
            Item item = WeaponQuickStart.Make(weaponData);
            item.ID = $"Shotgun{counter}";
            return item;
        }

        public static ParamEmitter MakeShotCluster(int level, float damage)
        {
            ParamEmitter emitter = new ParamEmitter();
            emitter.MinNumberOfGameObjects = 20;
            emitter.RangeNumberOfGameObject = 5;

            var expectedNumProjectiles = (emitter.RangeNumberOfGameObject * 0.5f) + emitter.MinNumberOfGameObjects;
            emitter.Emitter = MakeShot(level, damage / expectedNumProjectiles);
            
            emitter.VelocityAngleRange = 90;
            emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            emitter.VelocityMagMin = 10;
            emitter.VelocityMagRange = 5;
            emitter.RotationType = ParamEmitter.EmitterRotation.VelocityAngle;
            return emitter;
        }

        public static ProjectileProfile MakeShot(int level, float damage)
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.InitColor = new InitColorConst(Color.White); //new InitColorConst(ScalingUtils.EffectColor(level));
            profile.TextureID = "add8";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "25";
            profile.UpdateSize = null;
            profile.InitMaxLifetime = new InitFloatRandom(Utility.Frames(1), Utility.Frames(0.25f));
            profile.Mass = 0.1f;
            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name;

            profile.CollisionSpec = new CollisionSpec(damage, ImpactType.Additive, 0.5f);
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;           
            return profile;
        }
    }
}
