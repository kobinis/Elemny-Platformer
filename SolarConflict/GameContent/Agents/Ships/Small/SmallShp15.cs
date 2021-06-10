using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.Ships.Small
{
    class SmallShp15
    {
        public static GameObject Make()
        {
            ShipData data = new ShipData("SmallShip16", 0);
            data.Name = "RPS No Pirates Here";
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Durendal";
            ship.FactionType = Framework.FactionType.TradingGuild;

            //   ship.Light = Lights.HugeLight(Color.LightYellow);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(-68, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(1, 0), 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-32, -40), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-32, 40), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(97, -33), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(97, 33), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-92, -56), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-92, 56), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-33, 1), 0, ControlSignals.Action1);

            ShipQuickStart.AddBasicGearSlots(ship);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.impactSpec.IsDamaging = true;
            return ship;
        }
    }
}
