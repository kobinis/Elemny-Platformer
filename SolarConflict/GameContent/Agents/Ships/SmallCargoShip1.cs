using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolarConflict.GameContent.Ships {
    class SmallCargoShip1 {
        public static GameObject Make() {
            var ship = ShipQuickStart.Make("SmallCargoShip1", 0);
            ship.targetSelector.PrioritizeGoal = true;
            
            //ship.Multipliers.EngineAcceleration = 1.2f;
            //ship.Multipliers.EngineMaxSpeed = 1.2f;
            //ship.Multipliers.ShieldRegeneration = 0.8f;
            //ship.Multipliers.ShieldStrength = 0.8f;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Medium, new Vector2(-63, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Medium, new Vector2(15, -72), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Medium, new Vector2(15, 72), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Medium, new Vector2(114, 0), 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(0, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(65, -61), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(65, 61), 0, ControlSignals.Action2);

            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, 0);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, 0);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Rotation, 0);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, 0, ControlSignals.Action3);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            return ship;
        }
    }
}
