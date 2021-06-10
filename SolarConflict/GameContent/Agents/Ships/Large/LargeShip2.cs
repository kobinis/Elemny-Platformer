using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships.Large
{
    class LargeShip2
    {
        public static IEmitter Make()
        {
            var ship = ShipQuickStart.Make(new ShipData("LargeShip2", 0, inventorySize: 9 * 4, sizeType: SizeType.Large));
            ship.Name = "Jupiter";
            ship.FactionType = Framework.FactionType.Empire;
            AgentEngine engine1 = new AgentEngine(3, 30);
            engine1.activationCheck = new ActivationCheck(ControlSignals.Up);
            SystemHolder holder1 = new SystemHolder(engine1, new Vector2(-193, -24), MathHelper.Pi);
            ship.AddSystem(holder1);
            AgentEngine engine2 = new AgentEngine(3, 30);
            engine2.activationCheck = new ActivationCheck(ControlSignals.Up);
            SystemHolder holder2 = new SystemHolder(engine2, new Vector2(-193, 24), MathHelper.Pi);
            ship.AddSystem(holder2);

            //Guy's slots
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, ship.SizeType, new Vector2(-194, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-5, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(83, -105), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(83, 105), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-41, -105), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-41, 105), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(129, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-161, -77), 0, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(-161, 76), 0, ControlSignals.Action3);

            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Large, new Vector2(-194, 0), 180, ControlSignals.Up);            
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, new Vector2(-5, 0), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Large, new Vector2(-161, -77), -135, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Large, new Vector2(-161, 77), 135, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, new Vector2(83, -105), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, new Vector2(83, 105), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, new Vector2(-41, -105), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, new Vector2(-41, 105), 0, ControlSignals.Action1);
            //ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, SizeType.Large, new Vector2(129, 0), 0, ControlSignals.Action1);
            ShipQuickStart.AddBasicGearSlots(ship);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
