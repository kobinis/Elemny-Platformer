using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SolarConflict.AI;
using SolarConflict.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject;
using SolarConflict.Framework.Utils;
using SolarConflict.XnaUtils.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using XnaUtils.Graphics;

namespace SolarConflict
{
    [Serializable]
    public class AgentLoadout : IEmitter, IGameObjectFactory
    {                
        public int AiKey = 0; 
        public List<LoadoutEntry> LoadoutEntryList;        
        public List<InventoryItemEntery> _inventoryItems;

        public string ID { get; set; }
        public string Name { get; set; }
        public string LoadoutDescription { get; set; }

        [JsonIgnore]
        private Agent _agent;
        [JsonIgnore]
        public Agent Agent {
            get { return _agent; }
        }

        public string AgentID
        {
            get { return _agent.ID; }
            set { _agent = (Agent)ContentBank.Inst.GetEmitter(value); }
        }


        public Sprite GetSprite()
        {
            return _agent.Sprite;
        }

        [JsonIgnore]
        public string FullDescription
        {
            get
            {
                //\n{LoadoutDescription}
                return $"{Name ?? ID} ({_agent.SizeType})\n Price: {Cost}"
                    + AgentUtils.DescribeStatsAndAbilities(_agent, LoadoutEntryList.Select(e => Tuple.Create(e.Item, e.Activation))
                    .Where(t => t.Item1 != null));
            }
        }

        private float? _cost = null;

        [JsonIgnore]
        public float Cost
        {
            get
            {
                if (_cost == null)
                {
                    _cost = Agent.HullCost;
                    foreach (var loadoutEntry in LoadoutEntryList)
                    {
                        if (loadoutEntry.Item != null)
                            _cost += loadoutEntry.Item.Profile.BuyPrice;
                    }

                    foreach (var inventoryItemEntery in _inventoryItems)
                    {
                        if (inventoryItemEntery.Item != null)
                            _cost += inventoryItemEntery.Amount * inventoryItemEntery.Item.Profile.BuyPrice;
                    }
                }

                return _cost.Value;
            }
        }

        public void AddItemToInventory(string itemID, int amount = 1)
        {
            _cost = null;
            var item = ContentBank.Inst.GetItem(itemID, false);
            _inventoryItems.Add(new InventoryItemEntery(item, amount));
        }

        public void AddItemToSlot(int index, string itemID, ControlSignals? activation = null)
        {
            _cost = null;
            if (activation == null)
            {
                activation = _agent.ItemSlotsContainer[index].ActivationSignal;
            }
            LoadoutEntryList.Add(new LoadoutEntry(ContentBank.Inst.GetItem(itemID, false), activation.Value, index));
        }

        public void AddItemToSlot(int index, Item item, ControlSignals controlSignal = ControlSignals.None)
        {
            _cost = null;
            //if (controlSignal != null)
            LoadoutEntryList.Add(new LoadoutEntry(item, controlSignal, index));
        }
        
        public int Count
        {
            get { return LoadoutEntryList.Count; }
        }

        public AgentLoadout()
        {
            LoadoutEntryList = new List<LoadoutEntry>(); 
            _inventoryItems = new List<InventoryItemEntery>();
        }

        public AgentLoadout(Agent agent)
            : this()
        {            
            _agent = agent; // KOBI: get agent prototype from ContentBank
                            // TODO: test that ItemSlotsContainer and agent are not null and add relevant code.

            for (int i = 0; i < agent.ItemSlotsContainer.Count; i++)
            {
                ItemSlot itemSlot = agent.ItemSlotsContainer[i];
                if (itemSlot.Item != null)
                {
                    var prototypeItem = ContentBank.Inst.GetItem(itemSlot.Item.ID, false);
                    LoadoutEntry loadoutEntry = new LoadoutEntry(prototypeItem, itemSlot.ActivationSignal, i);
                    LoadoutEntryList.Add(loadoutEntry);
                }
                else
                {
                    LoadoutEntry loadoutEntry = new LoadoutEntry(null, itemSlot.ActivationSignal, i);
                    LoadoutEntryList.Add(loadoutEntry);
                }
            }            

            foreach (var inventoryItem in agent.Inventory.Items)
            {
                if (inventoryItem != null)
                {
                    var prototypeItem = ContentBank.Inst.GetItem(inventoryItem.ID, false);
                    InventoryItemEntery inventoryItemEntery = new InventoryItemEntery(prototypeItem, inventoryItem.Stack);
                    _inventoryItems.Add(inventoryItemEntery);
                }
            }
        }

        public void EquipInventory(Agent agent)
        {
            foreach (var inventoryItemEntery in _inventoryItems)
            {
                var item = inventoryItemEntery.Item.GetWorkingCopy();
                item.Stack = inventoryItemEntery.Amount;
                agent.Inventory.AddItem(item);
            }
        }

        public void EquipLoadout(Agent agent)
        {
           // agent.LoadoutID = this.ID;             
            if (AiKey != 0)
                agent.control.SetAIControl(AIBank.Inst.GetControl(AiKey));
            
            if (agent.ItemSlotsContainer != null) 
            {
                foreach (var loadoutEntry in LoadoutEntryList)
                {
                    Item item = null;

                    if (loadoutEntry.Item != null)
                    {
                        item = loadoutEntry.Item.GetWorkingCopy();
                    }

                    agent.ItemSlotsContainer[loadoutEntry.LocationID].Item = item; // TODO: maybe add ItemStack
                    agent.ItemSlotsContainer[loadoutEntry.LocationID].ActivationSignal = loadoutEntry.Activation;
                }
            }
            else
            {
                // TODO: Handle this invalid case
            }
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0) //add param?
        {
            var agentEmited = MakeGameObject(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color);
            gameEngine.AddList.Add(agentEmited);
            return agentEmited;
        }

        public GameObject MakeGameObject(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
           int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Agent agentEmited = this._agent.GetWorkingCopy();
            agentEmited.Name = Name;
            agentEmited.SetControlType(AgentControlType.AI);//??
            agentEmited.Parent = parent;
            agentEmited.FactionType = faction;
            agentEmited.Position = refPosition;
            agentEmited.Velocity = refVelocity;
            agentEmited.Rotation = refRotation;
            agentEmited.RotationSpeed = refRotationSpeed;
            agentEmited.Param = param;
            EquipLoadout(agentEmited);
            EquipInventory(agentEmited);
            return agentEmited;
        }

        public GameObject MakeGameObject(GameEngine gameEngine, GameObject parent = null, FactionType faction = FactionType.Neutral, int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            return MakeGameObject(gameEngine, parent, faction, Vector2.Zero, Vector2.Zero, 0, 0, param: param);           
        }
    }
}
