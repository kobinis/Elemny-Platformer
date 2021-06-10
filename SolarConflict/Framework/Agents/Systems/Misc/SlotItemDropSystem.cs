using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Agents.Systems
{
    [Serializable]
    class SlotItemDropSystem : AgentSystem
    {
        public ControlSignals Activation;
        public bool AlwaysDrop = true;
        public float Probability;

        public SlotItemDropSystem(ControlSignals activation, float probability = 1f, bool alwaysDrop = true)
        {
            Activation = activation;
            Probability = probability;
            AlwaysDrop = alwaysDrop;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            if((Activation & agent.ControlSignal) > 0 && agent.ItemSlotsContainer != null && agent.ItemSlotsContainer.Count > 0)
            {
                if (gameEngine.Rand.NextFloat() < Probability)
                    return false;

                if (!AlwaysDrop)
                {
                    // Randomly select a slot. If it's not empty, drop the contents
                    Item itemToDropCopy = agent.ItemSlotsContainer[gameEngine.Rand.Next(agent.ItemSlotsContainer.Count)].Item;
                    if (itemToDropCopy != null)
                    {
                        if (itemToDropCopy.Level == 0)
                            return false;
                        itemToDropCopy.Emit(gameEngine, agent, agent.FactionType, agent.Position, agent.Velocity, agent.Rotation, 0.1f);

                    }
                }
                else
                {
                    List<int> indexList = new List<int>();
                    for (int i = 0; i < agent.ItemSlotsContainer.Count; i++)
                    {
                        if (agent.ItemSlotsContainer[i].Item != null)
                        {
                            indexList.Add(i);
                        }
                    }
                    if(indexList.Count > 0)
                    {
                        // Randomly select an item to drop
                        int ind = indexList[gameEngine.Rand.Next(indexList.Count)];
                        Item itemToDropCopy = agent.ItemSlotsContainer[ind].Item;
                        if (itemToDropCopy.Level == 0)
                            return false;
                        itemToDropCopy.Emit(gameEngine, agent, agent.FactionType, agent.Position, agent.Velocity, agent.Rotation, 0.1f);
                    }
                }
                return true;
            }
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;
        }
    }
}
