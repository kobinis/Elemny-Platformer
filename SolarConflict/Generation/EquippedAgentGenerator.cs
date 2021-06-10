using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using System.Diagnostics;
using SolarConflict.AI;
using SolarConflict.AI.GameAI;

namespace SolarConflict.Generation
{
    [Serializable]
    public class EquippedAgentGenerator : IGameObjectFactory, IEmitter
    {
        [Serializable]
        private class SlotEntry
        {            
            public int LevelModifier;
            public int ReffranceIndex { get; private set; }
            public bool IsReffrencing { get { return items == null; } }
            public ControlSignals ControlSignal;
            private Dictionary<int, List<Item>> items;            
            //TODO: add flag - remake AI


            #region Constructors
            public SlotEntry()
            {
                items = new Dictionary<int, List<Item>>();
            }

            public SlotEntry(List<Item> itemList, ControlSignals controlSignal, int levelModifier = 0)
            {
                ControlSignal = controlSignal;
                items = new Dictionary<int, List<Item>>();
                LevelModifier = levelModifier;                
                foreach (var item in itemList)
                {
                    AddItem(item);
                }           
            }

            public SlotEntry(int reffranceIndex, ControlSignals controlSignal)
            {
                ReffranceIndex = reffranceIndex;
                ControlSignal = controlSignal;
            }
            #endregion

            public void AddItem(Item item, ItemCategory categoriesNotToInclude = ItemCategory.NonAI)
            {
                if((item.Category & categoriesNotToInclude) > 0)
                {
                    return;
                }
                if (!items.ContainsKey(item.Level)) 
                    items[item.Level] = new List<Item>();
                items[item.Level].Add(item);
            }

            public Item GetItem(int level, Random rand)
            {
                Debug.Assert(!IsReffrencing);
                if (items.Count == 0) return null;
                for (int curLevel = level+LevelModifier; curLevel >= 0; curLevel--)
                {
                    List<Item> itemList = null;
                    if(items.TryGetValue(curLevel, out itemList))
                    {
                        return itemList[rand.Next(itemList.Count)];
                    }
                }
                return null;
            }    

        }

        public string ID { get; set; }
        private SlotEntry[] slotEntries;
        private Agent agentFactory;
        public string AgentID { get { return agentFactory?.ID; } set { agentFactory = ContentBank.Inst.GetGameObjectFactory(value) as Agent; } }
        public SizeType SizeType { get { return agentFactory.SizeType; } }
        public string Name;
        public int AIKey = 0;

        public EquippedAgentGenerator(string agentID):this(ContentBank.Inst.GetGameObjectFactory(agentID) as Agent)
        {          
        }

        public EquippedAgentGenerator(Agent agentFactory)
        {
            this.agentFactory = agentFactory;
            int count = agentFactory.ItemSlotsContainer.Count;
            slotEntries = new SlotEntry[count];
        }

        public void SetSlotItems(int index, List<Item> items, ControlSignals controlSignal, int levelModefier = 0)
        {
            slotEntries[index] = new SlotEntry(items, controlSignal, levelModefier);
        }

        public void SetSlotItems(int index, int reffranceIndex, ControlSignals controlSignal)
        {
            slotEntries[index] = new SlotEntry(reffranceIndex, controlSignal);
        }

        public void AddItemToSlot(int index, Item item)
        {
            if(slotEntries[index] == null)
            {
                slotEntries[index] = new SlotEntry();
            }
            slotEntries[index].AddItem(item);
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float level = 0)
        {
            if (level == 0)
                level = gameEngine.Level;
            var agent = MakeGameObject(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, level);
            gameEngine.AddList.Add(agent);
            return agent;
        }

        public GameObject MakeGameObject(GameEngine gameEngine, GameObject parent = null, FactionType faction = FactionType.Neutral, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float level = 0)
        {
            return MakeGameObject(gameEngine, parent, faction, Vector2.Zero, Vector2.Zero, 0, 0, maxLifetime, size, color, level);
        }

        public GameObject MakeGameObject(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float level = 0)
        {
            Agent agent = agentFactory.MakeGameObject(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color) as Agent;
            if(Name != null)
            {
                agent.Name = Name;
            }
            agent.Level = (int)level;
            for (int i = 0; i < slotEntries.Length; i++)
            {
                Item item = GetItem(i, (int)level, agent.ItemSlotsContainer, gameEngine.Rand);
                if(item != null && item.Profile.AmmoType != ItemCategory.None) //TODO: fix
                {
                    var ammoItems = ContentBank.Inst.GetAllItemsCopied(SlotType.Ammo);
                    ammoItems = ammoItems.Where(x => (x.Category & item.Profile.AmmoType) > 0).ToList();
                    if (ammoItems.Count > 0)
                    {
                        var ammo = ammoItems[gameEngine.Rand.Next(ammoItems.Count)];
                        agent.AddItemToInventory(ammo.ID, 10);
                    }
                }
                agent.ItemSlotsContainer[i].Item = item;
                if(slotEntries[i] != null)
                    agent.ItemSlotsContainer[i].ActivationSignal = slotEntries[i].ControlSignal;
            }
            if(AIKey > 0)
            {
                //agent.control.controlAi = AIBank.Inst.GetControl(AIKey);
                agent.control.controlAi = ParameterControl.MakeAIFromAgent(agent); //TODO: maybe adjust ranges of control on the go
                //Or maybe just control.Asjust()
            }
            else
            {

            }
            return agent;
           
        }

        private Item GetItem(int index, int level, ItemSlotsContainer slotsContainer, Random rand)
        {
            SlotEntry entry = slotEntries[index];
            if (entry == null)
                return null;
            if(entry.IsReffrencing)
            {
                return slotsContainer[entry.ReffranceIndex].Item?.GetWorkingCopy();
            }
            return entry.GetItem(level, rand)?.GetWorkingCopy();
        }

    }
}