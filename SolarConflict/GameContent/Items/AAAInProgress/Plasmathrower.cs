//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
//using SolarConflict.GameContent.Projectiles.AATested;
//using SolarConflict.GameContent.Utils;
//using SolarConflict.Generation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.AATestedItems
//{
//    class Plasmathrower
//    {
//        public static Item Make()
//        {
//            //, "Emits a cone of fire."
//            WeaponData data = new WeaponData("Plasmathrower", 4, "FlameItem");
//            data.ShotEmitter = MakeFlame(ScalingUtils.ScaleDamagePerFrame(data.ItemData.Level) * 0.4f);
//            //data.ItemData.SecounderyIconID = "lvl" + data.ItemData.Level.ToString();
//            data.EnergyCost = 10;
//            data.ShotSpeed = 25;
//            data.SoundEffectEmitterID = null;
//            data.Cooldown = 1;
//            data.EffectEmitterID = null;
//           // data.ShotColor = Color.LightBlue;
//            data.ItemData.BuyPrice = (int)(ScalingUtils.ScaleCost(data.ItemData.Level) * 3f);
//            Item item = WeaponQuickStart.Make(data);
//            item.Profile.Level = 0;
//            return item;
//        }

//        public static ProjectileProfile MakeFlame(float damage)
//        {
//            ProjectileProfile profile = new ProjectileProfile();
//            profile.DrawType = DrawType.Additive;
//            profile.InitColor = new InitColorConst(new Color(0.5f, 0.5f, 0.5f));
//            profile.ColorLogic = ColorUpdater.FadeOut;
//            profile.TextureID = "add2c";
//            profile.Draw = new ProjectileDrawRotateWithTime(0f, 0f, "add2c");
//            profile.CollisionWidth = profile.Sprite.Width - 5;
//            profile.InitSizeID = "15";
//            profile.UpdateSize = new UpdateSizeGrow(4, 1.01f);
//            profile.InitMaxLifetime = new InitFloatConst(50);
//            profile.Mass = 0.1f;
//            profile.VelocityInertia = 0.99f;
//            profile.CollisionSpec = new CollisionSpec(damage, 0f); //1
//            profile.IsDestroyedOnCollision = false;
//            profile.IsEffectedByForce = false;
//            //profile.ApplyTags("effect", "explosion", "medium");
//            //  profile.ApplyTags("energy", "medium");
//            profile.Light = Lights.MediumLight(new Color(100, 100, 255));
//            return profile;
//        }
//    }
//}
