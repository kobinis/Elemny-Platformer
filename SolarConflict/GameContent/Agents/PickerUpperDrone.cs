using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolarConflict.GameContent.Agents
{
    class PickerUpperDrone
    {
        public static GameObject Make()
        {
            var ship = ShipQuickStart.Make(new ShipData("echo-sprint", 1500, inventorySize: 2 * 9, sizeType: SizeType.Small));
            ship.gameObjectType &= ~GameObjectType.Ship;

            ship.control.controlAi = new EmergencySalvagerAI();

            // Built-in systems
            var engine = new AgentEngine(5, 20);
            engine.activationCheck.controlMask = ControlSignals.Up;

            ship.AddSystem(new SystemHolder(engine, Vector2.UnitX * -10, MathHelper.Pi));

            var rotationEngine = new AgentRotationEngine(10000f);
            ship.AddSystem(new SystemHolder(rotationEngine, Vector2.Zero, 0f));

            var modulator = new EmitterCallerSystem();

            modulator.EmitterID = typeof(ItemPullingAoe).Name;
            modulator.MaxLifetime = 30;
            modulator.CooldownTime = 30;
            modulator.EmitterSpeed = 0;
            
            ship.AddSystem(new SystemHolder(modulator, Vector2.Zero, 0f));

            return ship;
        }
    }
}
