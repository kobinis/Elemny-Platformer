using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Reflection;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.Ships
{
    //Ramming pirate ship
    class SmallShip13 //TODO: add sound to ramming front
    {
        public static GameObject Make()
        {
            ShipData data = new ShipData("SmallShip13", 0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Dart";
            ship.FactionType = Framework.FactionType.Federation;

            //ShipCommon.AddCommonSystems(ship);
            DecorationSystem wingLights = new DecorationSystem();
            wingLights.AddDecoration(new Vector2(-29, 82), 0);
            wingLights.AddDecoration(new Vector2(-29, -82), MathHelper.Pi);
            ship.AddSystem(wingLights);
                       
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Small, new Vector2(-30, -39), 0, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Utility, SizeType.Small, new Vector2(-30, 39), 0, ControlSignals.None);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(12, -22), -90, ControlSignals.Action3);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Weapon, SizeType.Small, new Vector2(12, 22), 90, ControlSignals.Action4);
            ship.ItemSlotsContainer.AddAgentSlot(SlotType.Engine, SizeType.Small, new Vector2(-40, 0), 180, ControlSignals.Action2);

            ShipQuickStart.AddBasicGearSlots(ship);
            
            return ship;
        }
    }
}
