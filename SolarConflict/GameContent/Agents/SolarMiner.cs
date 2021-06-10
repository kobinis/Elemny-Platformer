using Microsoft.Xna.Framework;
using SolarConflict.Framework.Emitters;
using SolarConflict.GameContent.Utils;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolarConflict.GameContent.Agents
{
    public class SolarMiner
    {
        public static GameObject Make()  
        {
            Agent ship = ShipQuickStart.Make("base5", 0);
            ship.DrawType = DrawType.Lit;
            //ship.gameObjectType &= ~GameObjectType.Ship;
            ship.Mass = 100000;            
            ship.AddSystem(new RotationFixer(0));
            MeterGenerator shiled = new MeterGenerator();
            shiled.MeterType = MeterType.Shield;
            shiled.MaxValue = 300;
            shiled.GenerationAmountPerSec = 1 * 60;
            ship.AddSystem(shiled);

            HierarchyEmitter emitter = new HierarchyEmitter();
            emitter.NumberOfObjects = 1;
            emitter.AddEmitter(typeof(DevastationEmitter).Name);
            EmitterCallerSystem destroyedRingSystem = new EmitterCallerSystem(ControlSignals.OnDestroyed, 0, Vector2.Zero, emitter);  // EmitterFireRing
            //destroyedRingSystem.
            ship.AddSystem(destroyedRingSystem);
        
            return ship;
        }
    }
}
