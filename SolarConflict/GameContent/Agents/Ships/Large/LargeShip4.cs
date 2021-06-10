//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Ships.Large
//{
//    /// <summary>Chassis for the pirate flagship</summary>
//    class LargeShip4
//    {
//        public static IEmitter Make()
//        {            
//            Agent ship = ShipQuickStart.Make("LargeShip4", 7200, 9 * 3, mass: 1.2f, armor: 1.5f);
//            ship.Multipliers.EngineAcceleration = 1.1f;
//            ship.Multipliers.EngineMaxSpeed = 1.3f;
//            ship.Multipliers.ShieldRegeneration = 1f;
//            ship.Multipliers.ShieldStrength = 1f;

//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine, SizeType.Large, new Vector2(-194, 0), 180, ControlSignals.Up);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine, SizeType.Large, new Vector2(-193, -24), 180, ControlSignals.Up, new Vector2(-145, -30));
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Engine, SizeType.Large, new Vector2(-193, 24), 180, ControlSignals.Up, new Vector2(-145, 30));

//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Turret, SizeType.Large, new Vector2(-5, 0), 0, ControlSignals.Action1);

//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Turret, SizeType.Large, new Vector2(-161, -77), -135, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Turret, SizeType.Large, new Vector2(-161, 77), 135, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Turret, SizeType.Large, new Vector2(83, -105), 0, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Turret, SizeType.Large, new Vector2(83, 105), 0, ControlSignals.Action1);

//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Turret, 0, new Vector2(-41, -105), 0, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Turret, 0, new Vector2(-41, 105), 0, ControlSignals.Action1);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon | Item.Category.Turret, 0, new Vector2(129, 0), 0, ControlSignals.Action1);

//            // Flange weapons
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, 0, new Vector2(-36, -227), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, 0, new Vector2(-36, 227), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, 0, new Vector2(102, -188), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, 0, new Vector2(102, 188), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, 0, new Vector2(182, -139), 0, ControlSignals.Action2);
//            ship.ItemSlotsContainer.AddAgentSlot(Item.Category.Weapon, 0, new Vector2(182, 139), 0, ControlSignals.Action2);

//            //Basic gear
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Generator, SizeType.Large);
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Shield, SizeType.Large);
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.RotationEngine, SizeType.Large);
//            ship.ItemSlotsContainer.AddBasicSlot(Item.Category.Utility, SizeType.Large);

//            return ship;
//        }
//    }
//}
