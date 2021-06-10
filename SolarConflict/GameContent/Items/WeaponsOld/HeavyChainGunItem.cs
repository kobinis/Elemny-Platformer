//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.NewContent.Projectiles;

//namespace SolarConflict.GameContent.Items
//{
//    class HeavyChainGunItem
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemQuickStart.Profile("Heavy Chain Gun", "Blasts a high energy fusion shot", 0,
//            "FlameItem", "turret1");
//            profile.SlotType = SlotType.Weapon;            
//            profile.IsWorkingOnlyInSlot = true;
//            profile.BuyPrice = 2000;
//            profile.SellPrice = 1000;

//            Item item = new Item(profile);
//            EmitterCallerSystem system = new EmitterCallerSystem(typeof(FusionShot).Name);
//            system.CooldownTime = 35;
//            system.ActiveTime = 20;
//            system.MidCooldownTime = 8;
//            system.velocity = Vector2.UnitX * 25; // change to get a float
//            system.SecondaryEmitterID = typeof(GunFlashFx).Name;
//            system.SecondarySize = 40;
//            system.secondaryVelocityMult = 0.1f;
//            system.ThirdEmitterID = "sound_shot1";
//            system.ActivationCheck.AddCost(MeterType.Energy, 75);
//            item.System = new TurretSystemHolder(system, Vector2.Zero, "turret1");
//            //profile.DescriptionText += ItemQuickStart.ExtendedDescription(system.CooldownTime, typeof(FusionShot).Name, 75);

//          //  profile.ApplyTags("gun", "medium");

//            return item;
//        }
//    }
//}
