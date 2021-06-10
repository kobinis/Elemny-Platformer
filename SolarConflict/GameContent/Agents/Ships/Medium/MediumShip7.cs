using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships.Medium
{
    class MediumShip7
    {
        public static IEmitter Make()
        {
            var data = new ShipData("MediumShip7", 0,inventorySize:9*5);
            //data.Mass = ShipQuickStart.DefaultMass(data) * 2f;
            var ship = ShipQuickStart.Make(data);
            ship.Name = "Shark";
            ship.FactionType = Framework.FactionType.Pirates1;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-108, 16), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-108, -16), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-64, 23), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-64, -23), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(9, 36), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(9, -36), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(58, 49), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(58, -49), 0, ControlSignals.Action1);

            ShipQuickStart.AddBasicGearSlots(ship, shieldNum: 2);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);

            return ship;
        }
    }
}
