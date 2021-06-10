using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using System;

namespace SolarConflict.Framework.Emitters
{
    [Serializable]
    public class AddSystemsEmitter:IEmitter
    {
        public List<AgentSystem> SystemsToAdd;
        public IEmitter Emitter;
        public string ID { get; set; }

        public AddSystemsEmitter()
        {
            SystemsToAdd = new List<AgentSystem>();
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            GameObject gameObject = Emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);            
            if(gameObject is Agent)
            {
                Agent agent = gameObject as Agent;
                foreach (var system in SystemsToAdd)
                {
                    agent.AddSystem(system.GetWorkingCopy());
                }
            }
            return gameObject;
        }
    }
}
