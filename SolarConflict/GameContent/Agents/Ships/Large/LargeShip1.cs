using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using XnaUtils.Graphics;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.Ships
{
    class LargeShip1
    {
        public static IEmitter Make()
        {
            var data = new ShipData("LargeShip1", 0); //LargeShip1
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Devastator";
            ship.FactionType = Framework.FactionType.MinerGuild;

            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(64, -162), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(64, 162), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-86, -112), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-86, 112), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(104, -77), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(104, 77), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-170, -88), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Turret, ship.SizeType, new Vector2(-170, 88), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(124, 0), 0, ControlSignals.Down);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon | SlotType.Turret, ship.SizeType, new Vector2(-1, 0), 0, ControlSignals.Action1);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-8, -184), 270, ControlSignals.Right);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine | SlotType.Weapon, ship.SizeType, new Vector2(-8, 184), 90, ControlSignals.Left);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.MainEngine, ship.SizeType, new Vector2(-96, 0), 180, ControlSignals.Up);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(181, -96), 0, ControlSignals.Action2);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, ship.SizeType, new Vector2(181, 96), 0, ControlSignals.Action2);
            //Basic gear
            ShipQuickStart.AddBasicGearSlots(ship);
            ship.ItemSlotsContainer.AddBasicSlot(SlotType.Fabricator, SizeType.Large, ControlSignals.AlwaysOn);

            //DecorationSystem decSys = new DecorationSystem();
            //decSys.Sprite = Sprite.Get("starbase-red-light");
            //decSys.deltaTime = 0.1f;
            //decSys.Scale = 1f;
            //decSys.AddDecoration(new Vector2(-141, -213), 0);
            //decSys.AddDecoration(new Vector2(22, -225), 1 / 6f);
            //decSys.AddDecoration(new Vector2(53, -242), 2 / 6f);

            //decSys.AddDecoration(new Vector2(53, 242), 3 / 6f);
            //decSys.AddDecoration(new Vector2(22, 225), 4 / 6f);
            //decSys.AddDecoration(new Vector2(-141, 213), 5 / 6f);
            //ship.AddSystem(decSys);
            ShipQuickStart.FinalizeShip(ship);
            return ship;
        }
    }
}
