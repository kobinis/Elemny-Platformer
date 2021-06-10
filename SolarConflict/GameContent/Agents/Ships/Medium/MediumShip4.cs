using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    class MediumShip4
    {
        public static IEmitter Make()
        {
            Agent ship = ShipQuickStart.Make("MediumShip4", 0);
            ship.FactionType = Framework.FactionType.Empire;
            ship.Name = "Artemis";

            //Guy's slots
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(-68, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(58, 0), 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-7, -105), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-7, 105), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(-11, -47), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(-11, 47), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(-73, -97), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(-74, 97), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(65, 84), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(66, -84), 0, ControlSignals.Action4);

            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(-68, 0), 180, ControlSignals.Up);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(58, 0), 0, ControlSignals.Down);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-7, -105), 270, ControlSignals.Right);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-7, 105), 90, ControlSignals.Left);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-73, -98), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-73, 98), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(90, -65), 0, ControlSignals.Action2);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(90, 65), 0, ControlSignals.Action2);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-11, -47), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-11, 47), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(1, 0), 0, ControlSignals.Action1);            
            ShipQuickStart.AddBasicGearSlots(ship);
            ShipQuickStart.FinalizeShip(ship);

            return ship;
        }
    }
}
