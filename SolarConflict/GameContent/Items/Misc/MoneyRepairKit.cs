//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    class MoneyRepairKit
//    {
        
//            private static int COOLDOWN_IN_SECONDS = 30;

//            public static Item Make()
//            {
//                ItemProfile profile = ItemQuickStart.Profile("Auto Money Repair Kit",
//                                      string.Format("Restores 600 HP and 300 Shield\nMoney Cost: 300\nCooldown: {0} seconds", COOLDOWN_IN_SECONDS), 0, "AutoMoneyRepairKit");
//                profile.SlotType = SlotType.Utility;
//                profile.IsWorkingInInventory = true;
//                profile.IsActivatable = true;
//                profile.IsConsumed = false;
//                profile.MaxStack = 50;                

//                Item item = new Item(profile);
//                EmitterCallerSystem repair = new EmitterCallerSystem(ControlSignals.OnLowHitpoints, 60 * COOLDOWN_IN_SECONDS, "EmitterPickupFx");
//                repair.ActivationCheck = new ActivationCheck();
//                repair.ActivationCheck.AddCost(MeterType.Money, 300);
//                //repair.ActivationCheck.AddCost(MeterType.RepiarCooldown, ShipCommon.repairCooldownTime);
//                repair.SelfImpactSpec = new CollisionSpec();
//                repair.SelfImpactSpec.AddEntry(MeterType.Hitpoints, 600);
//                repair.SelfImpactSpec.AddEntry(MeterType.Shield, 2000);
//                //emitter.SelfImpactSpec.AddEntry(MeterType.Shield, 200); 
//                //repair.SelfImpactSpec.AddEntry(MeterType.RepiarCooldown, 0, ImpactType.Min);
//                item.System = repair;
//                //systemGroup.AddSystem(revive, true, false);
//                return item;
//            }
        
//    }
//}
