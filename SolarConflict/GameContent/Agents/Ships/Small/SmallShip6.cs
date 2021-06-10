using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip6
    {
        public static GameObject Make()
        {
            ShipData data = new ShipData("SmallShip6", 0, inventorySize:9*3);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Kusanagi";
            ship.FactionType = Framework.FactionType.TradingGuild;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(6, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Small, new Vector2(-29, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Small, new Vector2(-10, 53), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Small, new Vector2(-10, -53), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Utility, SizeType.Small, new Vector2(16, -34), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Utility, SizeType.Small, new Vector2(16, 34), 0, ControlSignals.Action4);

            ShipQuickStart.AddBasicGearSlots(ship);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.impactSpec.IsDamaging = true;

            return ship;
        }
    }
}
