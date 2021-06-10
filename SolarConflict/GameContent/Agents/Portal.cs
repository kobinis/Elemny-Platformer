using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Agents
{
    class Portal
    {
        public static IEmitter Make()
        {
            //Agent ship = .InstituteQuickStart("portal1", float.MaxValue, 0);
            //ship.gameObjectType &= ~GameObjectType.Ship;
            //ship.VelocityInertia = 0.977f;
            //ship.RotationMass = 220;
            //ship.Mass = 10000;
            //ship.listType = CollisionType.Collide1;
            //ship.DrawType = DrawType.Alpha;
            //ship.ActivityName = "back";
            //ship.Message = "Press F to warp";
            //ship.impactSpec = new CollisionInfo();
            //return ship;
            //throw new NotImplementedException();
            Agent portal = new Agent();
            portal.Sprite = Sprite.Get("add15");
            portal.Size = 200;
            portal.collideWithMask = GameObjectType.None;
            portal.DrawType = DrawType.Additive;
            portal.AddSystem(new BasicEmitterCallerSystem(ControlSignals.AlwaysOn, "PortalFx"));
            return portal;
        }
    }
}
