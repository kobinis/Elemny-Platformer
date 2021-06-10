using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships.Medium
{
    class MediumShip5
    {
        /// <summary>Chassis for lancer</summary>
        public static IEmitter Make()
        {
            ShipData data = new ShipData("MediumShip5", 0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Einherjar";
            ship.FactionType = Framework.FactionType.Empire;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.MainEngine, ship.SizeType, new Vector2(-105, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-53, -137), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-53, 137), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(52, 0), 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-40, -75), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-40, 75), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(42, -52), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(42, 52), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Large, new Vector2(8, 0), 0, ControlSignals.Action1);
            ShipQuickStart.AddBasicGearSlots(ship, true, 1,2);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
