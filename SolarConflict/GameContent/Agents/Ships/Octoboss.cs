//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Ships
//{
//    class Octoboss
//    {
//        public static GameObject Make()
//        {
//            Agent ship = ShipQuickStart.Make("MediumShip2", 800, 9 * 3);
//            ship.AddSystem(new RotationFixer(0));
//            ship.control.SetAIControl(AIBank.Inst.GetControl(7));

//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Utility, (ItemSize)1, new Vector2(-42, 34), 180, ControlSignals.None);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Utility, (ItemSize)1, new Vector2(39, 32), 0, ControlSignals.None);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine | Item.Category.Weapon, (ItemSize)1, new Vector2(0, 106), 90, ControlSignals.Up);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine | Item.Category.Weapon, (ItemSize)1, new Vector2(0, -106), -90, ControlSignals.Down);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine | Item.Category.Weapon, (ItemSize)1, new Vector2(106, -0), 0, ControlSignals.Left);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine | Item.Category.Weapon, (ItemSize)1, new Vector2(-106, 0), 180, ControlSignals.Right);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, (ItemSize)2, new Vector2(0, 0), 90, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, (ItemSize)1, new Vector2(-66, -80), 180, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, (ItemSize)1, new Vector2(66, 80), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, (ItemSize)1, new Vector2(-66, 80), 180, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, (ItemSize)1, new Vector2(66, -80), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Utility, (ItemSize)1, new Vector2(0, -50), -90, ControlSignals.Action3);
//            //Basic Slots
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Generator, (ItemSize)2);
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Shield | Item.Category.Cloaking, (ItemSize)2);
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Utility, (ItemSize)2);
//            return ship;
//        }
//    }
//}
