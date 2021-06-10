using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip3
    {
        public static GameObject Make()
        {
            ShipData data = new ShipData("SmallShip3", 0, inventorySize:9*3);
            Agent ship = ShipQuickStart.Make(data);
            ship.FactionType = Framework.FactionType.Void;

         //   ship.Light = Lights.HugeLight(Color.LightYellow);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, SizeType.Small, new Vector2(-50, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine , SizeType.Small, new Vector2(10, 0), 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Engine, SizeType.Small, new Vector2(22, -50), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Engine, SizeType.Small, new Vector2(22, 50), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Medium, new Vector2(-16, 0), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(-25, -33), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(-25, 33), 0, ControlSignals.Action1);

            ShipQuickStart.AddBasicGearSlots(ship, utilityNum:2);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.impactSpec.IsDamaging = true;
            return ship;
        }
    }
}
