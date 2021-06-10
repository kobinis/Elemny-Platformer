using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Agents.Institites
{
    class Shipyard
    {
        public static GameObject Make()  // fix starport
        {
            ShipData data = new ShipData("shipyard", 0, inventorySize:0);
            Agent ship = ShipQuickStart.Make(data);
            ship.Name = "Shipyard";
            ShipQuickStart.AddBasicGearSlots(ship);
            ship.gameObjectType &= ~GameObjectType.PotentialTarget;
            return ship;
        }
    }
}
