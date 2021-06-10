using Microsoft.Xna.Framework;
using SolarConflict.AI;
using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SolarConflict.GameContent.Misc.Buildings {
    class SpaceLamp {
        public static IEmitter Make() { 
            ShipData shipData = new ShipData("light1", 100);
            Agent ship = ShipQuickStart.Make(shipData);

            //ship.Light = Lights.MakeLight(Color.Aquamarine, 3f, distanceForHalfIntensity: 2000f, exponent: 1);
            // ship.Light = Lights.LargeLight(Color.LightYellow);
            LightSystem system = new LightSystem();
            system.LightObject.Light = Lights.LargeLight(Color.LightYellow);
            ship.AddSystem(system);
            return ship;
        }
    }
}
