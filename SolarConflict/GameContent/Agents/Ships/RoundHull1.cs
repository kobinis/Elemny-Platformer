//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Ships
//{
//    class RoundHull1
//    {
//        public static GameObject Make()
//        {
//            Agent ship = ShipQuickStart.Make("SmallShip11", 200, true);
//            ship.FactionType = Framework.FactionType.Void;
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(0, 42), 90, ControlSignals.Up);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(0, -42), 270, ControlSignals.Down);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(-42, 0), 180, ControlSignals.Right);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(42, 0), 0, ControlSignals.Left);         
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(0, 0), 0, ControlSignals.Action1);
//            ShipQuickStart.AddBasicGearSlots(ship, false, utilityNum: 2);
//            ShipQuickStart.FinalizeShip(ship);
//            return ship;
//        }
//    }
//}
