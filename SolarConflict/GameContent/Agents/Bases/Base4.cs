using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.Bases
{
    class Base4
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("LargeBase4", 3500,true);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Sharur";
            ship.FactionType = Framework.FactionType.TradingGuild;

            ship.gameObjectType |= GameObjectType.Mothership;
            ship.CraftingStationType |= Framework.CraftingStationType.Mothership;
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-74, -170), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(89, -170), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-97, -83), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(112, -83), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-96, 87), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(113, 88), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-74, 174), 90, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(89, 174), 90, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(11, -94), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-248, 3), 180, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(188, 2), 0, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(0, 118), 90, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(0, 0), 0, ControlSignals.None);
            ShipQuickStart.AddBasicGearSlots(ship, false, 1, 1, 1, SizeType.Huge);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.impactSpec.IsDamaging = true;
            return ship;
        }
    }
}
