//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolarConflict.GameContent.Items.Demo
//{
//    class BlinkItem
//    {
//        public static Item Make()
//        {
//            WeaponData weaponData = new WeaponData("Sniper gun");
//            //weaponData.ItemData.Description = "Shots a strong and fast projectile";
//            weaponData.ItemData.IconID = "SniperItem";
//            weaponData.ItemData.EquippedTextureId = "turret1";
//            weaponData.ItemData.Level = 3;
//            weaponData.ActiveTime = 1;
//            weaponData.Cooldown = 30;
//            //float damage = (float)Math.Round(ScalingUtils.ScaleDamagePerSec(weaponData.ItemData.Level) * weaponData.Cooldown / 60f * 5f);
//            weaponData.ShotEmitter = MakeShot(300, 1);
//            weaponData.ShotLifetime = (int)(30f);
//            weaponData.EnergyCost = weaponData.ComputeEnergyActivationCost(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level)); //(float)Math.Round(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level) * weaponData.Cooldown / 60f * 2f); ;
//            //weaponData.IsTurreted = false;
//            weaponData.ShotSpeed = 100;
//            weaponData.ItemData.BuyPrice = ScalingUtils.ScaleCost(weaponData.ItemData.Level) * 2;
//            weaponData.KickbackForce = 0.3f;
//            //TODO: speed mult = 0;
//            Item item = WeaponQuickStart.Make(weaponData);
//            return item;
//        }


//        public static ProjectileProfile MakeShot(float damage, float force)
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.ColorLogic = ColorUpdater.FadeOutSlow;
//            profile.TextureID = "disrupter";
//            profile.CollisionWidth = profile.Sprite.Height - 5; // why -5?
//            profile.InitSizeID = "25";
//            profile.UpdateSize = null;
//            profile.InitMaxLifetimeID = Utility.Frames(1.5f).ToString();
//            profile.Mass = 40f;
//            //profile.CollisionSpec = new CollisionInfo(220f, 200f);            
//            profile.CollisionSpec = new CollisionSpec(damage, 20f);
//            // ^ TEMP. Knockback looks ridiculous with low-mass asteroids, and our physics can't handle massive ones
//            profile.IsDestroyedOnCollision = true;
//            profile.IsEffectedByForce = true;
//            // profile.ApplyTags(new Vector3(0.145f, 0.93f, 0.49f), "energy", "medium", "bright");
//            return profile;
//        }
//    }
//}
