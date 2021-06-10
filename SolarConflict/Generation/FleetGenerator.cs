using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Generation
{

    class FleetGenerator
    {

        public static List<Agent> GenerateFleet(GameEngine gameEngine, GameObject parent, string ships, int amount, int level)
        {
            var result = new List<Agent>();
            var emitterIDs = ships.Split(',');
            emitterIDs.Shuffle(gameEngine.Rand);
            for (int i = 0; i < amount; i++)
            {
                
                var emitter = ContentBank.Inst.TryGetEmitter(emitterIDs[i % emitterIDs.Length]);
                if (emitter != null)
                {
                    var agent = emitter.Emit(gameEngine, parent, parent.GetFactionType(), parent.Position, Vector2.Zero, 0, level) as Agent;
                    agent.SetTarget(parent, TargetType.Goal);
                    agent.AddSystem(new Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers.BlueprintEmitterSystem(agent.ID + "BlueprintPart"));
                    agent.AddSystem(new LootSystem()); //Now: add loot
                    result.Add(agent);
                }
            }

            return result;
        }


        public static List<Agent> GenerateShips(GameEngine gameEngine, GameObject parent, FactionType faction, int level)
        {
            var result = new List<Agent>();

            var factionGenData = MetaWorld.Inst.GetFaction(faction).GenerationData;
            var isManny = FMath.Rand.Next(3) == 1 && level > 2; //Manny low 
            int[] shipCountPerSize = GetBaseShipCountPerSize(level - FMath.Rand.Next(2), isManny);
            if (isManny)
            {
                level -= 4;
                
            }
            level = Math.Max(level, 1);
            for (int shipSize = 0; shipSize < shipCountPerSize.Length; shipSize++)
            {
                int shipCount = shipCountPerSize[shipSize];
                var agentFactorys = factionGenData.loadouts.GetRange(0, Math.Min( level * 2, factionGenData.loadouts.Count));
                if (agentFactorys.Count > 0)
                {
                    int offset = FMath.Rand.Next(agentFactorys.Count);
                    for (int i = 0; i < shipCount; i++)
                    {
                        int factoryIndex = (i + offset) % agentFactorys.Count;
                        var agent = (agentFactorys[factoryIndex] as IEmitter).Emit(gameEngine, parent, faction, parent.Position, Vector2.Zero, 0, Math.Max(level - FMath.Rand.Next(2), 1)) as Agent;
                        agent.SetTarget(parent, TargetType.Goal);
                        var lootSystem = new LootSystem();
                        agent.AddSystem(new LootSystem());
                        agent.AddSystem(new Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers.BlueprintEmitterSystem(agent.ID + "BlueprintPart"));
                        agent.Name = agent.Name ;
                        if (level > 0)
                            agent.Name += " MK " + level;
                        result.Add(agent);
                    }
                }
            }

            return result;
        }

        static int[] GetBaseShipCountPerSize(int level, bool isManny)
        {
            Dictionary<int, int[]> sizes = new Dictionary<int, int[]>
            {
                { 0, new int[] { 1,0,0 } },
                { 1, new int[] { 1,0,0 } },
                { 2, new int[] { 2,1,0 } },
                { 3, new int[] { 2,1,0 } },
                { 4, new int[] { 0,2,0 } },
                { 5, new int[] { 2,1,0 } },
                { 6, new int[] { 3,2,0 } },
                { 7, new int[] { 3,2,1 } },
            };

            Dictionary<int, int[]> sizsies2 = new Dictionary<int, int[]>
           {
                  { 0, new int[] { 1,0,0 } },
                  { 1, new int[] { 1,0,0 } },
                  { 2, new int[] { 4,0,0 } },
                  { 3, new int[] { 6,1,0 } },
                  { 4, new int[] { 6,3,0 } },
                  { 5, new int[] { 2,5,0 } },
                  { 6, new int[] { 7,4,0 } },
                  { 7, new int[] { 4,5,1 } },
              };

            level = Math.Max(Math.Min(level, sizes.Count - 1), 0);

            if (isManny)
                return sizsies2[level];

            return sizes[level];
        }
    }
}
