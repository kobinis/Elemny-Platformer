
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using System.Reflection;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.Agents
{
    class Base1
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("LargeBase1", 2500, true, inventorySize:9*20);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Citadel";
            ship.FactionType = Framework.FactionType.Empire;
            ship.gameObjectType |= GameObjectType.Mothership;      
            ship.CraftingStationType |= Framework.CraftingStationType.Mothership;            
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, (SizeType)(ship.SizeType + 1), new Vector2(0, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(112, -85), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(112, 85), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-112, -85), 180, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-112, 85), 180, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-89, -172), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-89, 172), 90, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(89, -172), 270, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(89, 172), 90, ControlSignals.None);
            ShipQuickStart.AddBasicGearSlots(ship, false, 2, 2, 0);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility | SlotType.Mothership, ship.SizeType);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility | SlotType.Mothership, ship.SizeType);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.impactSpec.IsDamaging = true;
            return ship;
        }
    }
}

