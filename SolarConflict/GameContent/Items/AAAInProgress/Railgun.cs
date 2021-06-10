﻿using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitFloat;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    /// <summary>
    /// Sniper gun - Shots a strong projectile on a 0.5 sec cooldown
    /// </summary>
    class Railgun
    {
        public static Item Make()
        {
            WeaponData weaponData = new WeaponData("Railgun");
            //weaponData.ItemData.Description = "Shots a strong and fast projectile";
            weaponData.ItemData.IconID = "SniperItem";
            weaponData.ItemData.EquippedTextureId = "turret1";
            weaponData.ItemData.Level = 4;
            weaponData.ActiveTime = 1;
            weaponData.Cooldown = 60;
            weaponData.WarmupTime = 20;
            weaponData.WarmupEmitterID = "EmitterPickupFx";
            //float damage = (float)Math.Round(ScalingUtils.ScaleDamagePerSec(weaponData.ItemData.Level) * weaponData.Cooldown / 60f * 5f);
            weaponData.ShotEmitter = MakeShot(200, 1);
            weaponData.ShotLifetime = (int)(30f);
            weaponData.EnergyCost = weaponData.ComputeEnergyActivationCost(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level)); //(float)Math.Round(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level) * weaponData.Cooldown / 60f * 2f); ;
            //weaponData.IsTurreted = false;
            weaponData.ShotSpeed = 100;
            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level) * 2;
            weaponData.KickbackForce = 1f;

            //TODO: speed mult = 0;
            Item item = WeaponQuickStart.Make(weaponData);
            item.Profile.Category |= ItemCategory.NonAI;
            //  item.Profile.Level = 0;
            return item;
        }


        public static ProjectileProfile MakeShot(float damage, float force)
        {
            ProjectileProfile profile = new ProjectileProfile();
            profile.DrawType = DrawType.Additive;
            profile.ColorLogic = ColorUpdater.FadeOutSlow;
            profile.TextureID = "disrupter";
            profile.CollisionWidth = profile.Sprite.Height - 5; // why -5?
            profile.InitSizeID = "25";
            profile.UpdateSize = null;
            profile.InitMaxLifetimeID = Utility.Frames(1.5f).ToString();
            profile.Mass = 40f;
            //profile.CollisionSpec = new CollisionInfo(220f, 200f);            
            profile.CollisionSpec = new CollisionSpec(damage, 20f);
            profile.UpdateEmitterID = "EmitterFxSmoke";
            profile.UpdateEmitterCooldownTime = 1;
            profile.IsDestroyedOnCollision = false;
            profile.IsEffectedByForce = true;
            // profile.ApplyTags(new Vector3(0.145f, 0.93f, 0.49f), "energy", "medium", "bright");
            return profile;
        }
    }


}

