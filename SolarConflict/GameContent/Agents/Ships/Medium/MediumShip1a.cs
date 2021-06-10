//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;

//namespace SolarConflict.GameContent.Ships {
//    /// <summary>Chassis for heavy escort</summary>
//    class MediumShip1a {
//        public static IEmitter Make() {
//            Agent ship = ShipQuickStart.Make("MediumShip1", Globals.DontTweak ? 1000f : 100f, 9 * 3, mass:1f);
//            //ItemSlots                 
//            // All the dual engine/weapon slots are a problem. Three size 2 weapons would be too strong, but size 1 engines are way too weak to have
//            // any use as maneuvering thrusters. Made 'em size 2 for the engines.
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Utility, (ItemSize)1, new Vector2(-85, -45), 0, ControlSignals.None);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Utility, (ItemSize)1, new Vector2(-85, 45), 0, ControlSignals.None);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine, (ItemSize)1, Vector2.UnitX * -100, 180, ControlSignals.Up);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine | Item.Category.Weapon, (ItemSize)1, Vector2.UnitY * 100, 90, ControlSignals.Left);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine | Item.Category.Weapon, (ItemSize)1, Vector2.UnitY * -100, -90, ControlSignals.Right);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Utility, 0, Vector2.UnitX * -6, 0, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Utility, (ItemSize)1, new Vector2(70, -65), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Utility, (ItemSize)1, new Vector2(70, 65), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Utility, 0, new Vector2(-36, -42), 0, ControlSignals.Action3);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Utility, 0, new Vector2(-36, 42), 0, ControlSignals.Action4);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine | Item.Category.Weapon, (ItemSize)1, Vector2.UnitX * 40, 0, ControlSignals.Down);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Utility, 0, -Vector2.UnitX * 52, 0, ControlSignals.Action2);
//            //Basic gear
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Generator, (ItemSize)1);
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Shield, (ItemSize)1);
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.RotationEngine, (ItemSize)1);
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Utility, (ItemSize)1);
//            return ship;
//        }

//    }
//}
