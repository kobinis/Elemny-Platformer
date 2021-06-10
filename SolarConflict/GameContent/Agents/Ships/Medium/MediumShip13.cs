using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.Ships.Medium
{
    class MediumShip13
    {
        public static IEmitter Make()
        {
            var data = new ShipData("MediumShip13", 0);
            data.Mass = ShipQuickStart.DefaultMass(data) * 1.25f;
            var ship = ShipQuickStart.Make(data);
            ship.Name = "Freedom";
            ship.FactionType = Framework.FactionType.Federation;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-98, 126), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-98, -126), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Utility, ship.SizeType, new Vector2(35, 0), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-17, 100), -12.5f, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-17, -100), 12.5f, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(60, 57), -10, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(60, -57), 10, ControlSignals.Action1);

            ShipQuickStart.AddBasicGearSlots(ship);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);

            return ship;            
        }
    }
}
