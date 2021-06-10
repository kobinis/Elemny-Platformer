//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.NewContent.Projectiles;
//using SolarConflict.GameContent.Activities.PromoAndTest;

//namespace SolarConflict.GameContent.Items {
//    class FastDarkProfilingGun {
//        public static Item Make() {
//            ItemProfile profile = ItemQuickStart.Profile("FastDarkProfilingGun", "Test", 0,
//            "FlameItem", "turret1");
//            profile.Category = Item.Category.Weapon;            
//            profile.IsWorkingOnlyInSlot = true;


//            Item item = new Item(profile);
//            AgentEmitter system = new AgentEmitter(typeof(DarkProfilingShot).Name);
//            system.CooldownTime = ProfilingScene.FastGunCooldown;

//            system.velocity = Vector2.UnitX * 25;
//            system.ThirdEmitterID = "sound_shot1";
//            item.MainSystem = new TurretSystemHolder(system, Vector2.Zero, "turret1");

//            return item;
//        }
//    }
//}
