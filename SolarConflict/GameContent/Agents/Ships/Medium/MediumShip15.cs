using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships.Medium
{
    class MediumShip15
    {
        /// <summary>Chassis for the Federation assault</summary>
        public static IEmitter Make()
        {
            var data = new ShipData("MediumShip15", 0);
            var ship = ShipQuickStart.Make(data);
            ship.Name = "Mjolnir";
            ship.FactionType = Framework.FactionType.TradingGuild;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-131, 57), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-131, -57), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, new Vector2(-103, 0), 180, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret | SlotType.Utility, SizeType.Small, new Vector2(-62, 49), 30, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret | SlotType.Utility, SizeType.Small, new Vector2(-62, -49), -30, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret | SlotType.Utility, SizeType.Small, new Vector2(7, 71), 30, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret | SlotType.Utility, SizeType.Small, new Vector2(7, -71), -30, ControlSignals.Action1);

            ShipQuickStart.AddBasicGearSlots(ship);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);

            return ship;
        }
    }
}
