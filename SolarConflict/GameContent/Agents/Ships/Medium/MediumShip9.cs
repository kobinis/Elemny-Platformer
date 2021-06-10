using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships.Medium
{
    class MediumShip9
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("MediumShip9", 0, true);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Disorder";
            ship.FactionType = Framework.FactionType.Pirates1;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(109, 0), 0, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-111, 0), 180, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(0, -105), 270, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(0, 76), 90, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-57, 0), 270, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(59, -2), 270, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-47, -57), 270, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-47, 57), 90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(48, -56), 270, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(48, 56), 90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Large, new Vector2(2, -12), 270, ControlSignals.Action1);
            ShipQuickStart.AddBasicGearSlots(ship, false, shieldNum:2);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
