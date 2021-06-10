using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships.Medium
{
    class MediumShip14
    {
        public static IEmitter Make()
        {
            var ship = ShipQuickStart.Make("MediumShip14", 0, false, 9 * 4);
            ship.Name = "Vengeance";
            ship.FactionType = Framework.FactionType.Pirates1;          
            
            var rammingFront = new BasicEmitterCallerSystem(ControlSignals.AlwaysOn, typeof(HeavyRammingFront).Name);            
            ship.AddAfterSystem(new SystemHolder(rammingFront, new Vector2(208, 0), 0));            
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-18, -82), 270, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-18, 82), 90, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.MainEngine, ship.SizeType, new Vector2(-151, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(49, -46), 180, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, ship.SizeType, new Vector2(49, 46), 180, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(-70, -74), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(-70, 74), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret | SlotType.Weapon, ship.SizeType, new Vector2(-78, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret | SlotType.Weapon, SizeType.Large, new Vector2(-6, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret | SlotType.Weapon, ship.SizeType, new Vector2(91, 0), 0, ControlSignals.Action1);
            ShipQuickStart.AddBasicGearSlots(ship, true, 1, 2, 1);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
