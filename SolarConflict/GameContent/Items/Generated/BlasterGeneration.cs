using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Generated
{
    class BlasterGeneration
    {
        const int shotsPerVolley = 4;

        public static IEmitter Make()
        {
            int skip = 1;
            for (int i = 0; i < ScalingUtils.NumLevels; i += skip)
                ContentBank.Inst.AddContent(MakeGun(i, "Blaster", "BlasterIcon", "AoeRingGun", false, i));

            skip = 3;
            int counter = 1;
            for (int i = 1; i < ScalingUtils.NumLevels; i += skip)
            {
                ContentBank.Inst.AddContent(MakeGun(i, "Berserk", "BerserkBlasterIcon", "AoeRingGun", true, counter));
                counter++;
            }


            return null;
        }

        public static Item MakeGun(int level, string baseId, string icon, string equippedTexture, bool isBerserk, int counter)
        {
            WeaponData weaponData = new WeaponData($"{baseId} {level}");
            weaponData.ItemData.IconID = icon;
            weaponData.ItemData.EquippedTextureId = equippedTexture;
            if (level > 0)
                weaponData.ItemData.SecounderyIconID = $"lvl{level}";
            weaponData.ItemData.EquippedTextureId = "turret1";
            weaponData.ItemData.Level = level;
            weaponData.ItemData.Category = ItemCategory.EnergyConsumingWeapon | ItemCategory.Gun;
            weaponData.MidCooldownTime = 4;
            weaponData.Cooldown = 20;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(level);
            weaponData.ActiveTime = Math.Min(level * 2 + 1, weaponData.MidCooldownTime * 4);
            var numShots = 1 + weaponData.ActiveTime / weaponData.MidCooldownTime;

            weaponData.ShotSpeed = 80; //Math.Min( Math.Max(45, level * 10), 90);
            weaponData.ShotLifetime = (int)(2100 / weaponData.ShotSpeed);// (int)(ScalingUtils.ScaleShotRange(level) / weaponData.ShotSpeed);
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(level);
            weaponData.EnergyCost = weaponData.ComputeEnergyActivationCost(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level));
            weaponData.ShotEmitter = MakeShot(level, weaponData, ScalingUtils.ScaleDamagePerFrame(level));
            if (isBerserk)
            {
                weaponData.ItemData.BuyPrice = (int)(1.5f * weaponData.ItemData.BuyPrice);
                weaponData.EnergyCost *= 3f;
                weaponData.Cooldown = 10;
            }

            Item item = WeaponQuickStart.Make(weaponData);
            item.ID = $"{baseId}{counter}";
            return item;
        }

        public static ProjectileProfile MakeShot(int level, WeaponData weaponData, float targetDPS)
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            // profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "GrayAdd1";
            profile.CollisionWidth = profile.Sprite.Width - 10;
            profile.InitSizeID = "40";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = "100";
            profile.Mass = 0.1f;


            var damagePerShot = weaponData.ComputeShotDamage(ScalingUtils.ScaleDamagePerFrame(level));

            //  profile.RotationLogic = new UpdateRotationForward();
            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name;
            profile.CollisionSpec = new CollisionSpec(damagePerShot, 0.5f);
            profile.InitColor = new InitColorConst(ScalingUtils.EffectColor(level));
            profile.Light = Lights.MediumLight(ScalingUtils.EffectColor(level));
            profile.IsDestroyedOnCollision = true;
            profile.IsEffectedByForce = false;
            return profile;
        }


    }
}
