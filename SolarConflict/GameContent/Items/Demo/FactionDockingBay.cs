//using Microsoft.Xna.Framework;
//using SolarConflict.Framework.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    public class FactionDockingBay
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemQuickStart.Profile("Faction Docking Bay", "Faction Docking bay", 0,
//                    "FactionDockingBay", "item3");
//            profile.Category = Item.Category.Utility;
//            profile.IsWorkingOnlyInSlot = true;
//            profile.IsActivatable = true;

//            Item item = new Item(profile);

//            //var groupEmiiter = new GroupEmitter();
//            //groupEmiiter.AddEmitter("Jaguar1");
//            //groupEmiiter.AddEmitter("GhostShip");
//            //groupEmiiter.EmitType = GroupEmitter.EmitterType.RandomOne;

//            //AgentEmitter fighterEmitter = new AgentEmitter();
//            //fighterEmitter.EmitterID = "FactionLoadoutEmitter";
//            //fighterEmitter.CooldownTime = 60 * 30;
//            //fighterEmitter.ActiveTime = 2; // kobi: add mid cooldown
//            //fighterEmitter.velocity = Vector2.UnitX * 10;
//            //fighterEmitter.ActivationCheck.AddCost(MeterType.ControlPoints, 1); //add money cost
//            //fighterEmitter.ActivationCheck.AddItemCost("Cobalt", 5); //add money cost
//            //fighterEmitter.ActivationCheck.AddItemCost("Gold", 5); //add money cost

//            DockingBaySystem system = new DockingBaySystem();
//            system.CooldownTime = Utility.Frames(20f);
//            system.IsUsingFactionLoadouts = true;
//            system.ActivationCheck = new ActivationCheck();
//          //  system.ActivationCheck.AddCost(MeterType.ControlPoints, 1); //add money cost
//            //system.ActivationCheck.AddItemCost("Cobalt", 5); //add money cost
//            //system.ActivationCheck.AddItemCost("Gold", 5); //add money cost


//            item.MainSystem = system;

//            return item;
//        }


//    }

    
//}
