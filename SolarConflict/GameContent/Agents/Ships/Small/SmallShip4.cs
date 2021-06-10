using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    /// <summary>Chassis for light patrol boats</summary>
    class SmallShip4
    {
        public static GameObject Make()
        {
            ShipData data = new ShipData("SmallShip4", 0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Hinder";
            ship.FactionType = Framework.FactionType.Pirates1;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.MainEngine, ship.SizeType, new Vector2(-57, -22), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.MainEngine, ship.SizeType, new Vector2(-57, 22), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-17, -63), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-17, 63), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(51, 0), 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(40, -33), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(40, 33), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-17, 0), 0, ControlSignals.Action1);
            ShipQuickStart.AddBasicGearSlots(ship, shieldNum:2);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
