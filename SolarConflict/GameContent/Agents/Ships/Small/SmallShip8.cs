using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    /// <summary>Chassis for interceptor</summary>
    class SmallShip8
    {
        public static GameObject Make()
        {
            Agent ship = ShipQuickStart.Make("SmallShip8", 0, false, 9 * 3);
            ship.Name = "Guardian";
            ship.FactionType = Framework.FactionType.Federation;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, 0, new Vector2(-52, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Engine, 0, new Vector2(33, 0), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, 0, new Vector2(-21, -37), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, 0, new Vector2(-21, 37), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret | SlotType.Weapon, SizeType.Medium, new Vector2(-8, 0), 0, ControlSignals.Action1);

            ShipQuickStart.AddBasicGearSlots(ship, shieldNum: 2);  
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
