using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.Bases
{
    class Base3
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("LargeBase3", 2500, true);
            Agent ship = ShipQuickStart.Make(data);
            ship.FactionType = Framework.FactionType.Void;

            ship.gameObjectType |= GameObjectType.Mothership;
            ship.CraftingStationType |= Framework.CraftingStationType.Mothership;
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(0, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-65, -164), 270, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-65, 164), 90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(63, -165), 270, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(63, 165), 90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-104, -78), 270, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-104, 78), 90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(104, -78), 270, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(104, 78), 90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Fabricator, ship.SizeType, new Vector2(95, 0), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Fabricator, ship.SizeType, new Vector2(-95, 0), 180, ControlSignals.Action3);
            ShipQuickStart.AddBasicGearSlots(ship, false, 2, 2, 2);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.impactSpec.IsDamaging = true;
            return ship;
        }
    }
}
