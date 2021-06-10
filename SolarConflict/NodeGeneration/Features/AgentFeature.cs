using SolarConflict.Framework.World.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolarConflict.GameWorld;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.NodeGeneration.Features
{
    /// <summary>
    /// Add an agent 
    /// </summary>
    class AgentFeature:GenerationFeature
    {        
        public IEmitter AgentEmitter { get; set;}
        public string AgenEmitterID {set { AgentEmitter = ContentBank.Inst.GetEmitter(value); } }
        private List<AgentSystem> systemsToAdd;
        public string AgentName;       

        public void AddAgentSystem(AgentSystem system)
        {
            if (systemsToAdd == null)
                systemsToAdd = new List<AgentSystem>();
            systemsToAdd.Add(system);
        }

        public override GameObject GenerationLogic(Scene scene, SceneGenerator generator)
        {
            int level = Math.Max(Level, 1);
            var agent = AgentEmitter.Emit(scene.GameEngine, null, Faction, Position, Vector2.Zero, Rotation, param: level) as Agent;
            if (AgentName != null)
                agent.Name = AgentName;
            //agent.Name = GenerateName(agent, scene);
            if (systemsToAdd != null)
            {
                foreach (var system in systemsToAdd)
                {
                    var systemCopy = system.GetWorkingCopy();
                    agent.AddSystem(systemCopy);
                }
            }            
            return agent;
        }

        //private string GenerateName(Agent agent, Scene scene)
        //{
        //    return null;
        //}
    }
}
