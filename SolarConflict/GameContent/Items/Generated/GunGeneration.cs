using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Generated
{
    class GunGeneration {
        public static void Make() {
            //MakeChainGuns();
           // MakeFusionBlasters();
           // MakeHeavyGuns();
        }

        /// <param name="projectileCollisionSizeOffset">Projectiles' CollisionSize (which isn't actually collision size) will be its width minus this</param>
        public static void MakeGun(int level, string id, string name, string description, string icon, string equippedTexture, float dpsMultiplier, float energyCostMultiplier,
            int cooldown, float knockback, Color projectileColor, int projectileSize, string projectileSprite, int projectileCollisionSizeOffset, string projectileTrailEmitter, bool isTurreted,
            SizeType size = SizeType.Medium) {
            
            var damagePerFrame = ScalingUtils.ScaleDamagePerFrame(level) * dpsMultiplier;
            var energyPerFrame = ScalingUtils.ScaleEnergyCostPerFrame(level) * energyCostMultiplier;

            var damagePerShot = damagePerFrame * (cooldown + 1);
            var energyPerShot = energyPerFrame * (cooldown + 1);

            // Projectile
            var projectile = new ProjectileProfile();
            projectile.DrawType = DrawType.Additive;
            projectile.ColorLogic = ColorUpdater.FadeOutSlow;
            projectile.TextureID = projectileSprite;
            projectile.InitColor = new InitColorConst(projectileColor);
            projectile.CollisionWidth = projectile.Sprite.Width - projectileCollisionSizeOffset;
            projectile.InitSizeID = projectileSize.ToString();
            projectile.UpdateSize = null;
            projectile.InitMaxLifetimeID = "200";
            projectile.Mass = 1f;
            projectile.CollisionSpec = new CollisionSpec(damagePerShot, knockback);
            projectile.IsDestroyedOnCollision = true;
            projectile.IsEffectedByForce = false;

            // Trail
            if (projectileTrailEmitter != null) {
                var trail = new ParamEmitter();
                trail.EmitterID = projectileTrailEmitter;
                trail.VelocityAngleBase = 180;
                trail.VelocityMagMin = 6;

                projectile.UpdateEmitter = trail;
            }

            // WeaponData
            var turretString = isTurreted ? " Turret" : "";
            var weaponData = new WeaponData($"{name}{turretString}{level}", 0, icon, equippedTexture);
            if (level > 0)
                weaponData.ItemData.SecounderyIconID = $"lvl{level}";
            weaponData.ShotEmitter = projectile;
            weaponData.KickbackForce = knockback;
            //weaponData.IsTurreted = isTurreted;
            weaponData.Cooldown = cooldown;
            weaponData.EnergyCost = energyPerShot;
            weaponData.ShotSpeed = 40;
            weaponData.ItemData.Level = level;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(level);
            weaponData.ItemData.SellRatio = 0.5f;
            weaponData.ItemData.Size = size;

            // Item
            if (turretString.Length > 0)
                turretString = turretString.Substring(1);
            var item = WeaponQuickStart.Make(weaponData);
            item.Profile.Id = $"{id}{turretString}{level}";
            item.Profile.DescriptionText = description;

            ContentBank.Inst.AddContent(item);
        }

        static void MakeChainGuns() {
            var startLevel = 0;
            var dpsMultiplier = 1f;
            var energyCostMultiplier = 1f;
            var knockback = 0.3f;

            var id = "_Chaingun";
            var name = "Chaingun";
            var description = "Rapid-fire gun";
            var icon = "RailGun0";
            var equippedTexture = "turret9";

            var colors = new Color[] {
                new Color(0.5f, 1f, 0f),
                new Color(1f, 0f, 0f),
                new Color(0f, 1f, 0f),
            };
            var projectileSizes = new int[] {
                20,
                20,
                20
            };
            var projectileSprites = new string[] {
                "add10",
                "add10",
                "add10"
            };
            var projectileCollisionSizeOffsets = new int[] {
                10,
                10,
                10
            };
            var projectileTrailEmitters = new string[] {
                null,
                null,
                null,
            };

            for (int i = 0; i < colors.Length; ++i) {
                var level = i + startLevel;                

                // Calc cooldown
                var baseCooldown = Utility.Frames(0.34f);
                var cooldownReductionPerLevel = ((float)baseCooldown) * 2 / (3 * ScalingUtils.NumLevels - 1); // linear; top-tier weapon will have 1/3rd the cooldown of bottom tier

                var cooldown = (int)Math.Round(baseCooldown - cooldownReductionPerLevel * level);

                // Make items
                MakeGun(level, id, name, description, icon, equippedTexture, dpsMultiplier, energyCostMultiplier, cooldown,
                    knockback, colors[i], projectileSizes[i], projectileSprites[i], projectileCollisionSizeOffsets[i], projectileTrailEmitters[i], true);
            }
        }

        static void MakeFusionBlasters() {
            var startLevel = 3;

            var colors = new Color[] {
                new Color(1f, 0f, 0f),
                new Color(0f, 1f, 0f),
                new Color(1f, 1f, 0f),
                new Color(0f, 1f, 1f),
            };            
            
            for (int i = 0; i < colors.Length; ++i) {
                    var level = i + startLevel;
                    MakeGun(level, "_FusionBlaster", "Fusion Blaster", "Fires bolts of high-energy nuclear plasma", "RailGun2", "turret1", 1.25f, 1.75f, Utility.Frames(0.34f), 0.3f, colors[i], 65, "Fusion1",
                        0, null, true);
                
            }
        }

        static void MakeHeavyGuns() {
            /*var startLevel = 7;

            var colors = new Color[] {
                new Color(1f, 0f, 0f),
                new Color(0f, 1f, 0f),
                new Color(1f, 1f, 0f),
                new Color(0f, 1f, 1f),
            };

            for (int i = 0; i < colors.Length; ++i) {
                var level = i + startLevel;


                MakeGun(level, "_HeavyGun", "HeavyGun", "Fires massive kinetic impactors", "RailGun4", "heavygun", 2f, 1f, Utility.Frames(2f), 3f, colors[i], 15, "fireball1",
                    0, typeof(HeavyShotTrail).Name, true);
            }*/
            for (int i = 0; i < ScalingUtils.NumLevels; ++i) {
                MakeGun(i, "_HeavyGun", "HeavyGun", "Fires massive kinetic impactors", "RailGun4", "heavygun", 2f, 1f, Utility.Frames(2f), 3f, ScalingUtils.EffectColor(i), 15, "fireball1",
                    0, typeof(HeavyShotTrail).Name, true, SizeType.Large);
            }
        }

    }
}
