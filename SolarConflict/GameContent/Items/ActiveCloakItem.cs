//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    class ActiveCloakItem
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemQuickStart.Profile("Active Cloaking Device", "Activate to cloak\nCooldown: 10 seconds", 0, "CloakingDevice1", "echo-sprint");
//            profile.SlotType = SlotType.Utility; 
//            profile.ItemSize = SizeType.Small;
//            profile.IsWorkingOnlyInSlot = true;
//            profile.BreaksCloaking = false;
//            profile.BuyPrice = 500;

//            Item item = new Item(profile);
//            EmitterCallerSystem cloakingDevice = new EmitterCallerSystem(ControlSignals.Action3, 10, "FxEmitterCloak");
//            cloakingDevice.ActivationCheck.AddCost(MeterType.Energy, 0);
//            cloakingDevice.SelfImpactSpec = new CollisionSpec();
//            cloakingDevice.SelfImpactSpec.ImpactList.Add(new MeterCollisionSpec(MeterType.Cloak, 1500, ImpactType.Max));            
//            item.MainSystem = cloakingDevice;
//            return item;
//        }
//    }
//}
