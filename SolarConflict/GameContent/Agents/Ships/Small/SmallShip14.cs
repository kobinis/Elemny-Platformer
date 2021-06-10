using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip14
    {
        public static GameObject Make()
        {
            Agent ship = ShipQuickStart.Make("SmallShip14", 0);
            ship.Name = "Nemesis";
            ship.FactionType = Framework.FactionType.Empire;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, (SizeType)0, new Vector2(-53, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, (SizeType)0, new Vector2(9, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, (SizeType)0, new Vector2(-17, -49), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, (SizeType)0, new Vector2(-17, 49), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, (SizeType)0, new Vector2(-22, 0), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, (SizeType)0, new Vector2(-49, -63), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, (SizeType)0, new Vector2(-49, 63), 180, ControlSignals.Up);

            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Rotation, (SizeType)1);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, (SizeType)1);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, (SizeType)1);

            return ship;
        }
    }
}
