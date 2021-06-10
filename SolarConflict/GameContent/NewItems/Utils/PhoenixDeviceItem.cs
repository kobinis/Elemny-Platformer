//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SolarConflict.NewContent.Projectiles;
//using SolarConflict.GameContent.Projectiles;

//namespace SolarConflict.GameContent.NewItems
//{
//    class PhoenixDeviceItem
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemQuickStart.Profile("Phoenix Device", "Revives your ship when it is destroyed.\nWorks from the inventory.", 3, "pnx", "item3");
//            profile.SlotType = SlotType.Consumable;                        
//            profile.IsActivatable = false;
//            profile.IsConsumed = true;
//            profile.MaxStack = 20;
//            profile.IsRetainedOnDeath = true;
//            profile.SellPrice = 100;
//            profile.BuyPrice = 100;
            
//            Item item = new Item(profile);

//            EmitterCallerSystem agentEmitter = new EmitterCallerSystem(ControlSignals.OnDestroyed, typeof(ProjEmpShockwave1).Name);
//            agentEmitter.SelfImpactSpec = new CollisionSpec();
//            //agentEmitter.SelfImpactSpec.AddEntry(MeterType.StunTime, 100, ImpactType.Max);


//            ReviveSystem revive = new ReviveSystem();
            
//            SystemGroup systemGroup = new SystemGroup();
//            systemGroup.AddSystem(revive, true, false);
//            systemGroup.AddSystem(agentEmitter, false, false);
                       
//            item.System = systemGroup;
//            //systemGroup.AddSystem(revive, true, false);
            

//            return item;
//        }
//    }
//}
