using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Items;
using SolarConflict.GameContent.Utils;

namespace SolarConflict.GameContent.Agents
{
    public class Roadster      
    {
        public static GameObject Make()  
        {
            ShipData data = new ShipData("roadster", 1500, inventorySize: 0);
            data.VelocityInertia = 0.99f;
            
            var tesla = ShipQuickStart.Make(data);
            tesla.RotationInertia = 1;

            return tesla;
        }
    }
}
