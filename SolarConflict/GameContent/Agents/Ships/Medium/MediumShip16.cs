using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.Ships.Medium
{
    class MediumShip16
    {
        public static IEmitter Make()
        {
            var data = new ShipData("MediumShip16", 0);
            var ship = ShipQuickStart.Make(data);
            ship.Name = "Pillar";
            ship.FactionType = Framework.FactionType.Empire;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-87, -62), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-88, 64), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-136, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(49, 0), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Large, new Vector2(-27, 92), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(-27, -92), 0, ControlSignals.Action1);

            ShipQuickStart.AddBasicGearSlots(ship);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);

            return ship;
        }
    }
}
