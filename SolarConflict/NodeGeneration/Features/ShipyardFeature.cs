using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.GameWorld;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Session.World.Generation.Features
{
    class ShipyardFeature:GenerationFeature
    {
        public IEmitter AgentEmitter { get; set; }
        public string AgentEmitterID { set { AgentEmitter = ContentBank.Inst.GetEmitter(value); } }
        public List<string> ships;
        //private int level;

        public ShipyardFeature()
        {
            AgentEmitterID = "Shipyard";
            ships = new List<string>();
            
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            Agent shipyard = AgentEmitter.Emit(scene.GameEngine, null, Faction, Position, Vector2.Zero, MathHelper.PiOver2) as Agent;
            if (shipyard != null)
            {
            //    shipyard.gameObjectType |= GameObjectType.Shipyard;
            ////    shipyard.collideWithMask = GameObjectType.Asteroid;
            //    ShopSystem system = new ShopSystem(); //Sells loadouts
            //    shipyard.InteractionSystem = system;//shipyard.AddSystem(system);
            //    foreach (var item in ships)
            //    {
            //        system.AddItem(item+"KitItem");
            //    }

            //    var faction = scene.GameEngine.GetFaction(Faction);
            //    foreach (var item in faction.GenerationData.loadouts) //Select 2
            //    {
            //        system.AddItem(item.ID + "KitItem");
            //    }
            }
           
            
            return shipyard;
        }
    }
}
