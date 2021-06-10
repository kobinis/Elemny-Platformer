using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.NewContent.Projectiles;
using SolarConflict.NewContent.Emitters;
using SolarConflict.AI;
using System.Reflection;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.Ships
{
    class MediumShip2
    {
        public static GameObject Make()
        {
            Agent ship = ShipQuickStart.Make("MediumShip2", 0);
            ship.AddSystem(new RotationFixer(0));
      //      ship.control.SetAIControl(AIBank.Inst.GetControl(7));

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Medium, new Vector2(-42, 34), 180, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Medium, new Vector2(39, 32), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Medium, new Vector2(0, 106), 90, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Medium, new Vector2(0, -106), -90, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Medium, new Vector2(106, -0), 0, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, SizeType.Medium, new Vector2(-106, 0), 180, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(0, 0), 90, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(-66, -80), 180, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(66, 80), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(-66, 80), 180, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Medium, new Vector2(66, -80), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Medium, new Vector2(0, -50), -90, ControlSignals.None);
            //Basic Slots
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, SizeType.Medium);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, SizeType.Medium);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, SizeType.Medium);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, SizeType.Medium);

            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            return ship;
        }
    }
}
