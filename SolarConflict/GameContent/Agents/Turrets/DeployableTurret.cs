using Microsoft.Xna.Framework;
using SolarConflict.AI;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolarConflict.GameContent.Agents
{
    class DeployableTurret
    {
        public static IEmitter Make() //TODO: add energy
        {
            ShipData shipData = new ShipData("TurretAgent1", 100);
            Agent ship = ShipQuickStart.Make(shipData);
            ship.gameObjectType |= GameObjectType.Turret;
            ship.Name = "Defense Turret";
            ship.SetIsControllable(false);            
            ship.targetSelector.SetAggroRange(2000, 4000, TargetType.Enemy);                
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, SizeType.Large, Vector2.Zero, 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(41, 0), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-41, 0), 180, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(0, 41), 90, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(0, -41), -90, ControlSignals.Action2);
            ShipQuickStart.AddBasicGearSlots(ship);
            return ship;
        }
    }
}
