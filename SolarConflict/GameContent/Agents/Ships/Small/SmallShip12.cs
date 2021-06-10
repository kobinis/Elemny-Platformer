using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Ships
{
    class SmallShip12
    {
        public static GameObject Make()
        {
            Agent ship = ShipQuickStart.Make("SmallShip12", 0, inventorySize: 9 * 4);
            ship.Name = "Carnwennan";
            ship.FactionType = Framework.FactionType.TradingGuild;
            ship.control.controlAi = new MinerAI();

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, (SizeType)1, new Vector2(17, -6), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, (SizeType)1, new Vector2(-57, 10), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, (SizeType)1, new Vector2(-8, 54), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, (SizeType)1, new Vector2(-8, -52), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, (SizeType)1, new Vector2(-22, -21), 0, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, (SizeType)1, new Vector2(22, 30), 0, ControlSignals.Down);

            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Generator, (SizeType)1);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Shield, (SizeType)1);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Rotation, (SizeType)1);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Utility, (SizeType)1);

            //var rammingFront = new AgentEmitter(typeof(RammingFront).Name);
            //rammingFront.ActivationCheck = null;
            //ship.AddAfterSystem(new SystemHolder(rammingFront, new Vector2(5, 58), 0));

            return ship;
        }
    }
}
