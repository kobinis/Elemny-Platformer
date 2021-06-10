//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SolarConflict.GameContent.Agents.Ships.Void
//{
//    class VoidRoundHull1
//    {
//        public static GameObject Make()
//        {
//            Agent ship = ShipQuickStart.Make("Void_Fiend", 0, true, inventorySize: 9 * 3);
//            ship.FactionType = Framework.FactionType.Void;
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(0, 42), 90, ControlSignals.Up);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(0, -42), 270, ControlSignals.Down);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(-42, 0), 180, ControlSignals.Right);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(42, 0), 0, ControlSignals.Left);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-32, -32) * 1.1f, 0, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-32, 32) * 1.1f, 0, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(32, -32) * 1.1f, 0, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(32, 32) * 1.1f, 0, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(0, 0), 0, ControlSignals.Action2);
//            ShipQuickStart.AddBasicGearSlots(ship, false, 1 ,2, utilityNum: 2);
//            ShipQuickStart.FinalizeShip(ship);
//            return ship;
//        }
//    }
//}
