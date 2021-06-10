using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip5
    {
        public static GameObject Make()
        {
            ShipData data = new ShipData("SmallShip5", 0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Caladbolg";
            ship.FactionType = Framework.FactionType.TradingGuild;
            // ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(44, 13), 0, ControlSignals.Action1);
            // ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(44, -13), 0, ControlSignals.Action1);

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.MainEngine, ship.SizeType, new Vector2(-30, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(15, -41), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(15, 41), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(46, -15), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(46, 15), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Medium, new Vector2(3, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-36, -44), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-36, 44), 0, ControlSignals.Action4);
            ShipQuickStart.AddBasicGearSlots(ship);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
