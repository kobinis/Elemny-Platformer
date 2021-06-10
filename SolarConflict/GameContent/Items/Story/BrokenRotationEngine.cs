//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items.Story
//{
//    class BrokenRotationEngine
//    {
//        public static Item Make()
//        {
//            //, "Control the rotation of your ship!\nThis one is broken"
//            ItemData data = new ItemData("Broken Gyro", 11, "BrokenEngine");
//            data.SlotType = SlotType.Rotation;
//            data.Category = ItemCategory.Rotation;
//            var item = ItemQuickStart.Make(data);
//            PhysicalEngineSystem engine = new PhysicalEngineSystem(3, 10);
//            engine.activationCheck.controlMask.Signals = ControlSignals.AlwaysOn;    
//            SystemHolder holder = new SystemHolder(engine, new Vector2(-3, -25), MathHelper.Pi);
//            item.System = holder;           
//            return item;
//        }
//    }
//}
