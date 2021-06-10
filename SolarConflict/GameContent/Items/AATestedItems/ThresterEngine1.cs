//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    class ThresterEngine1  
//    {
//        public static Item Make()
//        {
//            ItemProfile profile = ItemQuickStart.Profile("Thruster", "Pushes and rotates you", 0, "square");
//            profile.SlotType = SlotType.Thruster;
//            Item item = new Item(profile);
//            AgentEngine engine = new AgentEngine(ControlSignals.None, 2, 20, 0);
//            engine.IsRotating = true;
//            item.MainSystem = engine;            
//            return item;
//        }
//    }
//}
