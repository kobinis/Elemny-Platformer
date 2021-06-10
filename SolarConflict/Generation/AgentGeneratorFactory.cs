using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.ContentGeneration
{
    //TODO: add item levels and item size, add support to item list (to be used with faction items)
    //Takes a loadout and generates an EquippedAgentGenerator
    public static class AgentGeneratorFactory
    {        
        public static EquippedAgentGenerator MakeAgentGenerator(AgentLoadout loadout, List<Item> itemList)
        {
            if (itemList == null)
                itemList = ContentBank.Inst.GetAllItems();
            EquippedAgentGenerator agentGenerator = new EquippedAgentGenerator(loadout.Agent);
            agentGenerator.Name = loadout.ID;
            agentGenerator.ID = loadout.ID + "_Gen";
            agentGenerator.AIKey = loadout.AiKey;
            Dictionary<string, int> pastItemsToIndex = new Dictionary<string, int>();          
            foreach (var loadoutEntry in loadout.LoadoutEntryList)
            {
                if (loadoutEntry.ItemId == null)
                    continue;
                int index = loadoutEntry.LocationID;

                if(!pastItemsToIndex.ContainsKey(loadoutEntry.ItemId))
                {
                    ItemSlot slot = loadout.Agent.ItemSlotsContainer[loadoutEntry.LocationID];
                    var items = GetItemList(loadoutEntry.ItemId, slot, itemList);
                  //  Debug.Assert(items != null && items.Count > 0, "Must be more the zero items");
                    if(items != null && items.Count > 0)
                        pastItemsToIndex.Add(loadoutEntry.ItemId, loadoutEntry.LocationID);
                    agentGenerator.SetSlotItems(loadoutEntry.LocationID, items, loadoutEntry.Activation);                                   
                }   
                else
                {
                    agentGenerator.SetSlotItems(loadoutEntry.LocationID, pastItemsToIndex[loadoutEntry.ItemId], loadoutEntry.Activation);
                }                                           
            }
            return agentGenerator;
        }

        private static List<Item> GetItemList(string itemID, ItemSlot itemSlot, List<Item> itemList = null)
        {

            var item = ContentBank.Inst.GetItem(itemID, false);
            if (itemList == null)
            {
                itemList = ContentBank.Inst.GetAllItems();
            }
            /*if ((item.Category & (ItemCategory.Rotation | ItemCategory.Engine)) > 0)
                itemList = itemList.Where(x => x.ItemSize >= item.ItemSize).ToList();*/
            itemList = itemList.Where(x => itemSlot.CanEquip(x)).Where(x => (x.Category & item.Category) > 0).ToList();
            return itemList;
        }

        public static EquippedAgentGenerator MakeAgentGeneratorFromAgent(Agent agent, List<Item> itemList)
        {
            EquippedAgentGenerator agentGenerator = new EquippedAgentGenerator(agent);
            agentGenerator.Name = agent.ID;
            agentGenerator.ID = agent.ID + "_Gen";
            if (agent.ItemSlotsContainer == null)
                return agentGenerator;
            for (int i = 0; i < agent.ItemSlotsContainer.Count; i++)
            {
                var items = GetItemList(agent.ItemSlotsContainer[i], itemList);
                //  Debug.Assert(items != null && items.Count > 0, "Must be more the zero items");
                agentGenerator.SetSlotItems(i, items, agent.ItemSlotsContainer[i].ActivationSignal);
            }
            return agentGenerator;
        }

        private static List<Item> GetItemList(ItemSlot itemSlot, List<Item> itemList = null)
        {
            if (itemList == null)
            {
                itemList = ContentBank.Inst.GetAllItems();
            }
            if (itemSlot.Type == SlotType.Utility)
            {
                itemList = itemList.Where(x => itemSlot.CanEquip(x)).ToList();
            }
            else
            {
                itemList = itemList.Where(x => itemSlot.CanEquip(x) && (x.SlotType & SlotType.Utility) == 0).ToList();
            }
            return itemList;
        }
    }
}
