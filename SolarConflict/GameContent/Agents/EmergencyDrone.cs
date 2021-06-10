//using Microsoft.Xna.Framework;
//using SolarConflict.GameContent.Items.Generated;
//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Agents {
//    class EmergencyDrone {
//        public static GameObject Make() {
//            var ship = ShipQuickStart.Make(new ShipData("echo-sprint", 1500, inventorySize: 9, sizeType: SizeType.Small));
//            ship.gameObjectType &= ~GameObjectType.Ship;

//            ship.control.controlAi = new EmergencySalvagerAI();

//            // Built-in systems
//            var engine = new AgentEngine(5, 20);
//            engine.activationCheck.controlMask = ControlSignals.Up;

//            ship.AddSystem(new SystemHolder(engine, Vector2.UnitX * -10, MathHelper.Pi));

//            var rotationEngine = new AgentRotationEngine(10000f);
//            ship.AddSystem(new SystemHolder(rotationEngine, Vector2.Zero, 0f));

//            var laser = MiningLaserGeneration.MakeMiningLaserItem(0).System as EmitterCallerSystem;
//            laser.ActivationCheck = new ActivationCheck(ControlSignals.Action1);
//            ship.AddSystem(new SystemHolder(laser, Vector2.Zero, 0f));           

//            return ship;
//        }
//    }
//}
