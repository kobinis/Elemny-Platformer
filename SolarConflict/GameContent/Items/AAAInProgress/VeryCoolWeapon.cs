//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.Emitters;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolarConflict.GameContent.Items.AAAInProgress
//{
//    class VeryCoolWeapon
//    {
//        public static Item Make()
//        {
//            //, "Emits a cone of fire."
//            WeaponData data = new WeaponData("Fart gun", 4, "attention");
//            data.ShotEmitter = MakeShot(10, 1);
//            data.ItemData.SecounderyIconID = "lvl" + data.ItemData.Level.ToString();
//            data.EnergyCost = 10;
//            data.ShotSpeed = 0;
//            data.SoundEffectEmitterID = null;
//            data.Cooldown = 1;
//            data.EffectEmitterID = null;
//            data.WarmupEmitterID = "Artillery";
//            data.WarmupTime = 5;
//            data.ItemData.BuyPrice = (int)(ScalingUtils.ScaleCost(data.ItemData.Level) * 3f);
//            Item item = WeaponQuickStart.Make(data);
//            //item.Profile.Level = 0;
            
//            return item;
//        }

//        public static ProjectileProfile MakeShot(float damage, float force)
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.ColorLogic = new UpdateColorSwitch(Color.Gray, Color.Red);
//            profile.TextureID = "add11";
//            profile.CollisionWidth = profile.Sprite.Height;
//            profile.InitSizeID = "300";
//            profile.UpdateSize = null;
//            profile.InitMaxLifetimeID = Utility.Frames(1).ToString();
//            profile.Mass = 40f;
//            //profile.CollisionSpec = new CollisionInfo(220f, 200f);            
//            profile.CollisionSpec = new CollisionSpec(0, 0);
//            profile.IsDestroyedOnCollision = false;
//            profile.IsEffectedByForce = false;
//            profile.TimeOutEmitter = MakeSecounderyShot(damage, force);
//            profile.VelocityInertia = 0;
//            // profile.ApplyTags(new Vector3(0.145f, 0.93f, 0.49f), "energy", "medium", "bright");
//            return profile;
//        }

//        public static ProjectileProfile MakeSecounderyShot(float damage, float force)
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.ColorLogic = new UpdateColorSwitch(Color.Gray, Color.Red);
//            profile.TextureID = "bigshockwave";
//            profile.CollisionWidth = profile.Sprite.Height;
//            profile.InitSizeID = "300";
//            profile.UpdateSize = null;
//            profile.InitMaxLifetimeID = Utility.Frames(2).ToString();
//            profile.Mass = 40f;
//            //profile.CollisionSpec = new CollisionInfo(220f, 200f);            
//            profile.CollisionSpec = new CollisionSpec(damage, force);
//            profile.IsDestroyedOnCollision = false;
//            profile.IsEffectedByForce = false;
//            //profile.TimeOutEmitter =
//            // profile.ApplyTags(new Vector3(0.145f, 0.93f, 0.49f), "energy", "medium", "bright");
//            return profile;
//        }
//    }
//}
