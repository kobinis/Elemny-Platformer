using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Agents
{
    class RoundDroneHull
    {
        public static GameObject Make()
        {
            Agent ship = ShipQuickStart.Make("Light1", 1000, true);
            int rad = (int)ship.Size ;
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Medium, new Vector2(0, rad), 90, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Medium, new Vector2(0, -rad), 270, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Medium, new Vector2(-rad, 0), 180, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Medium, new Vector2(rad, 0), 0, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(0, 0), 0, ControlSignals.Action2);
            ShipQuickStart.AddBasicGearSlots(ship, false, utilityNum:3, size: SizeType.Large);
            ShipQuickStart.FinalizeShip(ship);
            ship.isControllable = false;
          //  ship.IsDockable = false;
            return ship;
        }
    }
}
