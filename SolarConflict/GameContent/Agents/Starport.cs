using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Agents
{
    class Starport
    {
        public static GameObject Make()  // fix starport
        {
            Agent ship = ShipQuickStart.Make("station", 1000000);
            ship.collideWithMask = GameObjectType.None; //??
            ship.Name = "Starport";
            ship._drawType = DrawType.Lit;
            ship.gameObjectType &= ~GameObjectType.Ship;
            ship.gameObjectType |= GameObjectType.PotentialTarget;
            ship.gameObjectType |= GameObjectType.NonRotating;
            ship.Mass = 100000;            
            ship.AddSystem(new RotationFixer(0));
            ship.RemoveSystem(ship.GetSystem<DamageTextEmitter>());            
            //MeterGenerator hitpoints = new MeterGenerator();
            //hitpoints.MeterType = MeterType.Hitpoints;
            //hitpoints.MaxValue = 50000;
            //hitpoints.GenerationAmountPerSec = 60;
            //ship.AddSystem(hitpoints);            
            return ship;                        
        }
    }
}
