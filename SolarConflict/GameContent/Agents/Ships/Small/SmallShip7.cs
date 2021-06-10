using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip7
    {
        public static GameObject Make()
        {
            ShipData data = new ShipData("SmallShip7", 0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Agent";
            ship.FactionType = Framework.FactionType.Empire;

            //Guy's slots
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-38, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(52, 0), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-27, -41), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-27, 41), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(0, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(3, -38), 270, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(2, 35), 90, ControlSignals.Right);

            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Small, new Vector2(-38, 0), 180, ControlSignals.Up);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(17, 40), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(17, -40), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(52, 0), 0, ControlSignals.Action2);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Small, new Vector2(-27, -41), 0, ControlSignals.Action3);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Small, new Vector2(-27, 41), 0, ControlSignals.Action4);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Small, new Vector2(0, 0), 0, ControlSignals.Action1)


            ShipQuickStart.AddBasicGearSlots(ship);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
