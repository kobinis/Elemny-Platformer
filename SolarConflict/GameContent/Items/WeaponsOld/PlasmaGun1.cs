//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Projectiles;
//using SolarConflict.NewContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    class PlasmaGun1
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemQuickStart.Profile("Plasma Gun", "Blasts a high energy plasma shot", 0,
//            "05", null);
//            profile.BreaksCloaking = true;
//            profile.SlotType = SlotType.Weapon | SlotType.Turret;                        
//            profile.BuyPrice = 2500; //1500;
//            profile.SellPrice = 1250;//750;
//            profile.MaxStack = 1;


//            Item item = new Item(profile);
//            EmitterCallerSystem system = new EmitterCallerSystem(typeof(PlasmaShot1).Name);            
//            system.CooldownTime = 20;
//            system.velocity = Vector2.UnitX * 40; // change to get a float
//            system.SecondaryEmitterID = typeof(GunFlashFx).Name;
//            system.SecondarySize = 40;
//            system.secondaryVelocityMult = 0.1f; //Kobi: change secondaryVelocityMult to Velocity
//            system.ThirdEmitterID = "sound_shot1";
//            system.ActivationCheck.AddCost(MeterType.Energy, 25);

//            item.System = new TurretSystemHolder(system, Vector2.Zero, "turret1");
//           // profile.DescriptionText += ItemQuickStart.ExtendedDescription(system.CooldownTime, typeof(PlasmaShot1).Name, 25);

//            return item;
//        }
//    }
//}
