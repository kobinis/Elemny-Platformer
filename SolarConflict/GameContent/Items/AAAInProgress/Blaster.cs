//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using SolarConflict.NewContent.Emitters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolarConflict.GameContent.Items
//{
//    class Blaster
//    {
//        public static Item Make()
//        {
//            WeaponData weaponData = new WeaponData("Blaster");
//            weaponData.ItemData.IconID = "Blaster";
//            weaponData.ItemData.EquippedTextureId = "turret1";
//            weaponData.ItemData.Level = 10;

//            weaponData.Range = 6000;
//            weaponData.ShotSpeed = 80;
//            weaponData.ShotLifetime = (int)(weaponData.Range / weaponData.ShotSpeed);
//            weaponData.Cooldown = 30;
//            weaponData.ActiveTime = 1;
//            float damage = 40;
//            weaponData.ShotEmitter = MakeShot(damage, 1);
//            weaponData.EnergyCost = (float)Math.Round(ScalingUtils.ScaleEnergyCostPerFrame(weaponData.ItemData.Level) * weaponData.Cooldown / 60f * 2f); ;
//            //2 weaponData.IsTurreted = false;
//            weaponData.ItemData.BuyPrice = 200;
//            weaponData.KickbackForce = 0.1f;
//           // weaponData.EffectEmitterID = "EmitterFxSmoke";
//            //TODO: speed mult = 0;
//            Item item = WeaponQuickStart.Make(weaponData);
//            item.Profile.Level = 0;
//            return item;
//        }


//        public static ProjectileProfile MakeShot(float damage, float force)
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.Light = Lights.SmallLight(Color.Yellow);
//            profile.DrawType = DrawType.Additive;
//            profile.ColorLogic = ColorUpdater.FadeOutSlow;
//            profile.TextureID = "add10";
//            profile.CollisionWidth = profile.Sprite.Width - 10;
//            profile.InitSizeID = "20";
//            profile.UpdateSize = null;
//            profile.InitMaxLifetimeID = "100";
//            profile.Mass = 0.1f;
//            //  profile.RotationLogic = new UpdateRotationForward();
//            profile.ImpactEmitterID = typeof(EmitterImpactFx1).Name;
//            profile.CollisionSpec = new CollisionSpec(damage, force);
//            profile.IsDestroyedOnCollision = true;
//            profile.IsEffectedByForce = false;
//            return profile;
//        }
//    }
//}
