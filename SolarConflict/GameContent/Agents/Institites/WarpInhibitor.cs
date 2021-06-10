using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.Institites
{
    class WarpInhibitor
    {
        public static IEmitter Make()
        {
            ShipData data = new ShipData("WarpInhibitor", 1500, true);
            //data.IsAddCommonSystems = false;
            Agent ship = ShipQuickStart.Make(data);
            ship.DrawType = DrawType.Lit;
            ship.Name = "Warp Inhibitor";
            ship.FactionType = Framework.FactionType.Void;
            ship.AddSystem(new BasicEmitterCallerSystem(ControlSignals.OnDestroyed, "FullExplosionFx1"));

            ship.AddSystem(new RotationFixer(0));

            float sacle = 0.5f;
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(356, 259)*sacle, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(217, 547) * sacle, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(481, 339) * sacle, 0, ControlSignals.Action2);

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-356, 259)*sacle, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-217, 547)*sacle, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-481, 339) * sacle, 0, ControlSignals.Action2);

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(356, -259)*sacle, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(217, -547)*sacle, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(481, -339) * sacle, 0, ControlSignals.Action2);

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-356, -259)*sacle, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-217, -547)*sacle, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-481, -339) * sacle, 0, ControlSignals.Action2);

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(0, 0) * sacle, 0, ControlSignals.Action3);

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(420, 0) * sacle, 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-420, 0) * sacle, 180, ControlSignals.Action4);

            ShipQuickStart.AddBasicGearSlots(ship, false, 2, 2);
            ship.HullCost = AgentUtils.CalculateAgentHullCost(ship);
            ship.impactSpec = new CollisionSpec(0, 6);
            ship.impactSpec.IsDamaging = true;
            return ship;
        }
    }
}
