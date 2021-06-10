//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Emitters;
//using SolarConflict.GameContent.Projectiles;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    class MissileLauncherItem
//    {
//        public static Item Make()
//        {

//            ItemProfile profile = ItemQuickStart.Profile("Missile Launcher", "Launches the missiles from your top leftmost slot in your inventory", 0, "RocketLauncherItem", "turret1");
//            profile.SlotType = SlotType.Weapon;            
//            profile.IsWorkingOnlyInSlot = true;
//            profile.BuyPrice = 6000;
//            profile.SellPrice = 3000;
//            profile.MaxStack = 1;

//            ItemGroupEmitter ammoEmitter = new ItemGroupEmitter();
//            ammoEmitter.AddItem("MissileItem");
//            ammoEmitter.AddItem("BlastMissileItem");
//            ammoEmitter.AddItem("EMPMissileItem");
//            ammoEmitter.AddItem("Goo DronesMissileItem");
//            ammoEmitter.AddItem("Energy DrainMissileItem");
//            ammoEmitter.AddItem("Gravity WellMissileItem");
//            //"Aoe", "Emp", "Goo", "EnergyDrain", "GravityWell"

//            Item item = new Item(profile);
//            EmitterCallerSystem system = new EmitterCallerSystem();
//            system.activateOnlyOnEmit = true;
//            system.ActivationEmitter = ammoEmitter;
//            system.CooldownTime = 30;
//            system.EmitterSpeed = 2;
//            system.SecondaryEmitterID = typeof(GunFlashFx).Name;
//            system.secondaryVelocityMult = 0.1f;
//            system.ThirdEmitterID = "sound_shot1"; //Sound effect
//            //system.ActivationCheck.AddCost(MeterType.Shield, 30); //Energy Cost

//            item.MainSystem = system;            
//            return item;
//        }
//    }
//}
