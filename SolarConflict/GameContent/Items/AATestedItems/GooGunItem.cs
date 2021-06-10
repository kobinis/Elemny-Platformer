//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Projectiles;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.Temp
//{
//    class GooGunItem
//    {
//        public static Item Make()
//        {

//            ItemProfile profile = ItemQuickStart.Profile("Goo Gun", "Shots homing balls of goo that slow the enemy", 0, "LaserItem", "turret1");
//            profile.Category = Item.CategoryType.Weapon;
//            profile.ItemSize = Item.SizeType.Small;
//            profile.IsWorkingOnlyInSlot = true;            

//            Item item = new Item(profile);
//            AgentEmitter system = new AgentEmitter(typeof(GooBall).Name);
//            system.CooldownTime = 30;
//            system.EmitterSpeed = 10;
//            system.SecondaryEmitterID = typeof(GunFlashFx).Name;
//            system.secondaryVelocityMult = 0.1f;
//            system.ThirdEmitterID = "sound_shot1";

//            item.MainSystem = system;            
//            return item;
//        }
//    }
//}
