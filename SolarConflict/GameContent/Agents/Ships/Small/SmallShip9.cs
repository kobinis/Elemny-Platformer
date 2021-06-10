using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip9
    {
        public static GameObject Make()
        {
            ShipData data = new ShipData("SmallShip9", 0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Gandiva";
            ship.FactionType = Framework.FactionType.TradingGuild;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(3, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Small, new Vector2(-44, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Small, new Vector2(-5, -58), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Small, new Vector2(-5, 58), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Small, new Vector2(-40, -35), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Small, new Vector2(-40, 35), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(58, -33), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(58, 33), 0, ControlSignals.Action2);

            ShipQuickStart.AddBasicGearSlots(ship, true, 2);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.impactSpec.IsDamaging = true;
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
