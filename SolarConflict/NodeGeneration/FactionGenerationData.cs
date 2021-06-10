using SolarConflict.GameContent.ContentGeneration;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.NodeGeneration
{
    [Serializable]
    public class FactionGenerationData
    {
        public string MothershipID;
        private List<Item> _shipItems;
        public IEnumerable<Item> ShipItems => _shipItems;        
        public List<IGameObjectFactory> loadouts;
        private List<EquippedAgentGenerator> agentGenerators;  

        public FactionGenerationData()
        {
            _shipItems = new List<Item>();
            loadouts = new List<IGameObjectFactory>();

            // Add common items
            AddItems(ItemCategory.Engine);
            AddItems(ItemCategory.Rotation);
            AddItems(ItemCategory.Generator);
            AddItems(ItemCategory.Shield);
            AddItems(ItemCategory.Gun, ItemCategory.Shotgun);            
        }

        public void AddItem(string itemID)
        {
            agentGenerators = null;
            _shipItems.Add(ContentBank.Inst.GetItem(itemID, false));
        }

        public void AddItems(ItemCategory category, ItemCategory notFlags = ItemCategory.None)
        {
            agentGenerators = null;
            var items = ContentBank.Inst.GetAllItems();
            foreach (var item in items)
            {
                if (Item.IsItemInCategory(item, category, notFlags))
                {
                    _shipItems.Add(item);
                }
            }
        }

        public void ClearAllItems()
        {
            _shipItems.Clear();
        }

        public void AddLoadout(string loadoutID)
        {
            agentGenerators = null;
            loadouts.Add(ContentBank.Inst.GetLoadout(loadoutID));
        }

        public void AddLoadouts(string loadoutList)
        {
            agentGenerators = null;
            var loadoutSplit = loadoutList.Split(',');
            foreach (var loadoutID in loadoutSplit)
            {
                loadouts.Add(ContentBank.Inst.GetGameObjectFactory(loadoutID));
            }          
        }

        public List<IGameObjectFactory> GetAgentGenerators()
        {
            return loadouts;
            //if (agentGenerators == null)
            //{
            //    agentGenerators = new List<IGameObjectFactory>();
            //    foreach (var loadout in loadouts)
            //    {
            //        agentGenerators = loadout;
            //    }
            //}
            //return agentGenerators;
        }

        //public List<EquippedAgentGenerator> GetAgentGeneratorsBySize(SizeType size)
        //{
        //    var agentGens = GetAgentGenerators(); 
        //    return agentGens.Where(x => x.SizeType == size).ToList();
        //}

    }
}
