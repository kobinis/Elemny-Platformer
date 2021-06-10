using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Reflection;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.Ships 
{
    class MediumShip1
    {
        public static IEmitter Make()
        {            
            var ship = ShipQuickStart.Make(new ShipData("MediumShip1", 0, inventorySize: 9 * 3, sizeType: SizeType.Medium));
            ship.Name = "Gram";
            ship.FactionType = Framework.FactionType.TradingGuild;

            //ItemSlots                 
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Large, new Vector2(-85, -45), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Large, new Vector2(-85, 45), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine , SizeType.Large, Vector2.UnitX * -100, 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Medium, Vector2.UnitY * 100, 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Medium, Vector2.UnitY * -100, -90, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, Vector2.UnitX * -6, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Utility, SizeType.Large, new Vector2(70, -65), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Utility, SizeType.Large, new Vector2(70, 65), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret , SizeType.Large, new Vector2(-36, -42), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, new Vector2(-36, 42), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Medium, Vector2.UnitX * 40, 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Utility, SizeType.Large, -Vector2.UnitX * 52, 0, ControlSignals.Action2);            

            //Basic gear
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, SizeType.Large);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Large);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Rotation, SizeType.Large);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Large);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            return ship;
        }

    }
}
