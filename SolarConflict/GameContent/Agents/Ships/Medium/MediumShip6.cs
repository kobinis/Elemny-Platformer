using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships.Medium
{
    /// <summary>Chassis for the standoff bomber</summary>
    class MediumShip6
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("MediumShip6", 0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Aegis";
            ship.FactionType = Framework.FactionType.TradingGuild;

            //Guy's slots
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(-94, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(56, 0), 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(27, -56), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(27, 56), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-34, 70), 90, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-34, -70), -90, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(84, 40), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(84, -40), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-92, -82), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-92, 82), 0, ControlSignals.Action4);


            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(-94, 0), 180, ControlSignals.Up);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(56, 0), 0, ControlSignals.Down);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(27, -56), 270, ControlSignals.Right);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(27, 56), 90, ControlSignals.Left);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-34, 70), 90, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-34, -70), -90, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(84, 40), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(84, -40), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-92, -82), 0, ControlSignals.Action3);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-92, 82), 0, ControlSignals.Action4);

            ShipQuickStart.AddBasicGearSlots(ship);
            return ship;
        }
    }
}
