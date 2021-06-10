using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.Institites
{
    class Arena
    {
        public static GameObject Make()  // fix starport
        {
            ShipData data = new ShipData("base5", 0, inventorySize: 0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Arena";
            //ShipQuickStart.AddBasicGearSlots(ship);
            return ship;
        }
    }
}
