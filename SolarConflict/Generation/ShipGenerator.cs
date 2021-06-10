using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using System.Diagnostics;
using SolarConflict.AI;
using SolarConflict.AI.GameAI;
using Newtonsoft.Json;
using SolarConflict.XnaUtils;
using System.IO;
using SolarConflict.XnaUtils.Files;

namespace SolarConflict.Generation
{
    [Serializable]
    public class ShipGenerator : IGameObjectFactory, IEmitter
    {
        [Serializable]
        public class SlotEntry
        {
            /// <summary>
            /// Exspend the selected level from level to level+LevelRange at random
            /// </summary>
            [JsonIgnore]            
            public int LevelRange;//Currently not used
            public int ReferenceIndex { get; set; }
            public ControlSignals ControlSignal;
            public Dictionary<int, List<string>> ItemsPerLevel;
            public int Index;

            #region Constructors

            public SlotEntry()
            {                
                ItemsPerLevel = new Dictionary<int, List<string>>();
                ReferenceIndex = -1;            
            }

            public SlotEntry(List<Item> items, ControlSignals controlSignals, int index):this()
            {
                Index = index;
                ControlSignal = controlSignals;
                foreach (var item in items)
                {
                    AddItem(item.Level, item.ID);
                }
            }

            public SlotEntry(int level, string itemList, ControlSignals controlSignal, int index)
            {
                Index = index;
                ControlSignal = controlSignal;
                Index = index;
            }

            public SlotEntry(int reffranceIndex, ControlSignals controlSignal, int index)
            {
                Index = index;
                ReferenceIndex = reffranceIndex;
                ControlSignal = controlSignal;
                if (reffranceIndex >= 0)
                    ItemsPerLevel = null;
            }
            #endregion

            public void AddItemsToLevel(int level, string itemList)
            {                
                var itemIDList = itemList.Split(',');
                foreach (var id in itemIDList)
                {
                    AddItem(level, id);
                }
            }

            public void AddItem(int level, string itemID)
            {
                if (!ItemsPerLevel.ContainsKey(level))
                    ItemsPerLevel[level] = new List<string>();
                ItemsPerLevel[level].Add(itemID);
            }

            public string GetItemID(int level, Random rand)
            {                
                if (ItemsPerLevel.Count == 0) return null;
                int currentLevel = level;
                List<string> items;
                do
                {
                    ItemsPerLevel.TryGetValue(currentLevel, out items);
                    currentLevel--;
                } while (items == null && currentLevel >= 0);
              
                if (items != null && items.Count > 0)
                {
                    return items[rand.Next(items.Count)];
                }
                
                return null;
            }

        }

        public string ID { get; set; }        
        public string AgentID { get { return _agentFactory?.ID; } set { _agentFactory = ContentBank.Inst.GetGameObjectFactory(value) as Agent; } }        
        public string Name;
        public int AIKey = 0; //Generate AI 
        public SlotEntry[] SlotEntries;

        [JsonIgnore]
        private Agent _agentFactory;


        public ShipGenerator()
        {

        }

        public ShipGenerator(string agentID)
        {
            AgentID = agentID;
            int count = _agentFactory.ItemSlotsContainer.Count;
            SlotEntries = new SlotEntry[count];
        }
                

        //public void SetSlotItems(int index, int reffranceIndex, ControlSignals controlSignal)
        //{
        //    SlotEntries[index] = new SlotEntry(reffranceIndex, controlSignal);
        //}

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
            Agent agent = _agentFactory.MakeGameObject(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color) as Agent;
            if (Name != null)
            {
                agent.Name = Name;
            }
            agent.Level = (int)level;
            for (int i = 0; i < SlotEntries.Length; i++)
            {
                Item item = GetItem(i, (int)level, agent.ItemSlotsContainer, gameEngine.Rand);
                if (item != null && item.Profile.AmmoType != ItemCategory.None) //TODO: fix
                {
                    var ammoItems = ContentBank.Inst.GetAllItemsCopied(SlotType.Ammo);
                    ammoItems = ammoItems.Where(x => (x.Category & item.Profile.AmmoType) > 0).Where(x => x.Level <= level).ToList();
                    if (ammoItems.Count > 0)
                    {
                        var ammo = ammoItems[gameEngine.Rand.Next(ammoItems.Count)];
                        agent.AddItemToInventory(ammo.ID, 10);
                    }
                }
                agent.ItemSlotsContainer[i].Item = item;
                if (SlotEntries[i] != null)
                    agent.ItemSlotsContainer[i].ActivationSignal = SlotEntries[i].ControlSignal;
            }
            if (AIKey == 0)
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
            SlotEntry entry = SlotEntries[index];
            if (entry == null)
                return null;
            if (entry.ReferenceIndex >= 0)
            {
                return slotsContainer[entry.ReferenceIndex].Item?.GetWorkingCopy();
            }
            var itemID = entry.GetItemID(level, rand);
            if(itemID != null)
                return ContentBank.Inst.GetItem(itemID, true);
            return null;
        }


        public static ShipGenerator MakeShipGenerator(string agentID, List<Item> itemList = null)
        {
            if (itemList == null)
                itemList = ContentBank.Inst.GetAllItems();
            ShipGenerator generator = new ShipGenerator(agentID);
            generator.ID = agentID + "_sg";
            ItemSlotsContainer container = generator._agentFactory.ItemSlotsContainer;
            int indexOfFirstEngine = -1;
            for (int i = 0; i < container.Count; i++)
            {                
                List<Item> filtredItems = new List<Item>();
                ItemSlot slot = container[i];

                //list.Sort((x, y) => x.m_valRating.CompareTo(y.m_valRating));

                filtredItems = itemList.Where(item => slot.CanEquip(item) && (slot.Type & item.SlotType) > 0 && item.Level <= 10).ToList();
                filtredItems = filtredItems.OrderBy(it => it.Level).ToList();
                if ((slot.Type & (SlotType.Engine | SlotType.MainEngine)) > 0)
                {                    
                    if (indexOfFirstEngine == -1)
                    {
                        indexOfFirstEngine = i;
                        generator.SlotEntries[i] = new SlotEntry(filtredItems, slot.ActivationSignal, i);
                    }
                    else
                    {
                        generator.SlotEntries[i] = new SlotEntry(indexOfFirstEngine, slot.ActivationSignal, i);
                    }
                }
                else
                {
                    generator.SlotEntries[i] = new SlotEntry(filtredItems, slot.ActivationSignal, i);
                }
            }
            return generator;
        }

        public static void MakeAllShipGenerators()
        {
            var agents = ContentBank.Inst.GetAllAgents();
            foreach (var agent in agents)
            {
                if((agent.gameObjectType & GameObjectType.Ship) > 0  && agent.ItemSlotsContainer != null)
                {
                    var generator = MakeShipGenerator(agent.ID);
                    ContentBank.Inst.AddContent(generator, true);

                    SaveLoadManager.Instance().Save(Path.Combine(Consts.GAME_DATA_PATH, "ShipGenerators", generator.ID + ".json"), generator);
                }

            }
        }

        public static void LoadShipGenerators()
        {
            var files = FileUtils.GetFiles(Path.Combine(Consts.GAME_DATA_PATH, "ShipGenerators"), "*.json");
            foreach (var file in files)
            {
                var generator = SaveLoadManager.Instance().Load<ShipGenerator>(file);
                ContentBank.Inst.AddContent(generator, true);
            }            
        }
    }
}
