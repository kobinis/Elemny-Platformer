using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships.Medium
{
    class MediumShip8
    {
        public static IEmitter Make()
        {
            var data = new ShipData("MediumShip", 0);
            
            var ship = ShipQuickStart.Make(data);
            ship.Name = "Marksman";
            ship.FactionType = Framework.FactionType.Federation;
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-90, 62), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-90, -62), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret | SlotType.Utility, ship.SizeType, new Vector2(-63, -58), -90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret | SlotType.Utility, ship.SizeType, new Vector2(-63, 58), 90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret | SlotType.Engine, SizeType.Small, new Vector2(30, -44), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret | SlotType.Engine, SizeType.Small, new Vector2(30, 44), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-7, 0), 0, ControlSignals.Action1);

            ShipQuickStart.AddBasicGearSlots(ship);

            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);

            return ship;
        }
    }
}
