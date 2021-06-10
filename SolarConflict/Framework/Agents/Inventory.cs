using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict
{
    // TODO: make ItemSlotsContainer derived  from list
    [Serializable]
    public class  Inventory //add indexers 
    {
        public const int ROW_SIZE = 9; //8
        public const int QUICK_USE_COUNT = 4;
        public static readonly ControlSignals[] quickUseBind = new ControlSignals[QUICK_USE_COUNT] 
            { ControlSignals.QuickUse1, ControlSignals.QuickUse2, ControlSignals.QuickUse3, ControlSignals.QuickUse4 };

        #region Fields
      //  Dictionary<string, int> itemCount; //maybe int to int, item IdNum to item stack
        public Item[] Items;        
       
        #endregion

        #region Constructors
        public Inventory(int size)
        {            
            Items = new Item[size];
        }
        #endregion

        #region Properties
        public int Size
        {
            get { return Items.Length; }
        }
        #endregion

        #region Public Methods

        public static void CountItems(IEnumerable<Inventory> inventoryList, Dictionary<string, int> itemCount)
        {
            itemCount.Clear();
            foreach (var inventory in inventoryList)
            {
                if (inventory == null) //??
                    continue;
                for (int i = 0; i < inventory.Size; i++)
                {
                    Item item = inventory.GetItem(i);
                    if (item != null && item.ID != null)
                    {
                        if (itemCount.ContainsKey(item.ID))
                        {
                            itemCount[item.ID] += item.Stack;
                        }
                        else
                        {
                            itemCount[item.ID] = item.Stack;
                        }
                    }
                }
            }            
        }

        /// <summary>Compress inventory into as few stacks as possible, without repositioning stacks</summary>
        public void Stack() {
            var stacksWithRoom = new Dictionary<string, List<int>>();

            for (int i = 0; i < Items.Length; ++i) {
                if (Items[i] == null || Items[i].MaxStack <= 1)
                    // Nothing stackable
                    continue;

                var currentStack = Items[i];

                if (!stacksWithRoom.ContainsKey(currentStack.ID))
                    stacksWithRoom[currentStack.ID] = new List<int>();
                var currentItemStacksWithRoom = stacksWithRoom[currentStack.ID];

                // Found a stack after one or more non-empty stack of the same type; transfer stuff from current stack to prior ones until it's empty or they're full
                while (currentItemStacksWithRoom.Count > 0 && currentStack.Stack > 0) {
                    var rearmostStack = Items[currentItemStacksWithRoom[0]];

                    var transferred = Math.Min(rearmostStack.MaxStack - rearmostStack.Stack, currentStack.Stack);
                    rearmostStack.Stack += transferred;
                    currentStack.Stack -= transferred;

                    if (rearmostStack.Stack == rearmostStack.MaxStack)
                        // Rearmost stack full, pop it
                        currentItemStacksWithRoom.RemoveAt(0);
                }

                if (currentStack.Stack < currentStack.MaxStack)
                    currentItemStacksWithRoom.Add(i);
            }
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null && Items[i].Stack == 0)
                    Items[i] = null;
            }
        }

        public bool HasRoom(string itemId, int amount) //maybe return the index it can fit in
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Item.CanStack(Items[i], itemId, amount))
                {
                    return true;
                }      
            }

            return false;
        }

        ///// <summary>Returns the number of stacks of the given item which can be added to the inventory, _without_ compressing existing stacks</summary>        
        //public int RoomAvailable(Item item) {
        //    return Items.Sum(i => {
        //        if (i == null)
        //            return item.MaxStack;

        //        if (i.ID == item.ID)
        //            return i.MaxStack - i.Stack;

        //        return 0;
        //    });
        //}

        public void Clear(string itemId = null)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (itemId ==null || (Items[i] != null && Items[i].ID == itemId) )
                {
                    Items[i] = null;
                }
            }
        }        

        public bool RemoveItem(string itemId, int neededAmount = 1)
        {
            int amount = 0;
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null)
                {
                    if (Items[i].ID == itemId)
                    {
                        int amountToTake= Math.Min(Items[i].Stack, neededAmount - amount);
                        Items[i].Stack -= amountToTake;
                        amount += amountToTake;
                        if (amount >= neededAmount)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int GetItemCount(string itemId)
        {
            int amount = 0;
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null && Items[i].ID == itemId)
                {
                    amount += Items[i].Stack;
                }
            }
            return amount;
        }

        public bool CheckForItem(string itemId, int neededAmount) 
        {
            int amount = 0; 
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null)
                {
                    if (Items[i].ID == itemId)
                    {
                        amount += Items[i].Stack;
                        if (amount >= neededAmount)
                        {
                            return true;
                        }
                    }
                }              
            }
            return false;
        }

        public int CountItem(string itemId)
        {
            int amount = 0;
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null)
                {
                    if (Items[i].ID == itemId)
                    {
                        amount += Items[i].Stack;                        
                    }
                }
            }
            return amount;
        }

        public bool AddItem(string itemID) 
        {
            return AddItem(ContentBank.Inst.GetItem(itemID, true));
        }


        public bool AddItem(Item item) 
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == null || Items[i].Stack == 0 )
                {
                    item.IsActive = false;
                    Items[i] = item;
                    return true;
                }
                else
                {
                    if (Items[i].StakItem(item))
                    {
                        return true;
                    }
                }                
            }
            return false;
        }

        public bool AddItemFromEnd(Item item)
        {
            for (int i = Items.Length-1; i >= 0 ; i--)
            {
                if (Items[i] == null)
                {
                    item.IsActive = false;
                    Items[i] = item;
                    // itemsInInventory++;
                    return true;
                }
                else
                {
                    if (Items[i].StakItem(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Item GetItem(int n) //change to indexer
        {
            if (n < Items.Length)
                return Items[n];
            return null;
        }

        public int IndexOfFirstEmptySlot()
        {
            for (int i = 0; i < Items.Length; ++i)
                if (Items[i] == null || Items[i].Stack == 0)
                    return i;
            // No empty slots
            return -1;
        }

        public Item SetItem(int n, Item item) // change to indexer
        {
            Item invItem = Items[n];
            Items[n] = item;
            return invItem;
        }


        public Item RemoveItem(int n)
        {
            if (n < Items.Length && Items[n] != null)
            {
                Item item = Items[n];
                Items[n] = null;
               // itemsInInventory--; //remove
                return item;
            }
            return null;
        }

        /// <summary>Transfers as much of a given stack as possible to another inventory</summary>
        /// <returns>True if the entire given stack was successfully transferred</returns>
        public bool TryTransfer(Inventory recipient, int itemIndex) {           
            var item = Items[itemIndex];
            if (item == null)
                return true;
            bool wasItemAddedInFull = recipient.AddItem(item);
            if (wasItemAddedInFull)
            {
                Items[itemIndex] = null;
            }
            return wasItemAddedInFull;
        }

        /// <returns>True if all items in the appropriate category were successfully transferred</returns>
        public bool TryTransfer(Inventory recipient, ItemCategory categories) {
            var success = true;

            for (int i = 0; i < Items.Length; ++i)
                if (Items[i] != null && (Items[i].Category & categories) > 0)
                    success = TryTransfer(recipient, i) && success;

            return success;
        }

        /// <returns>True if all items in the appropriate category were successfully transferred</returns>
        public bool TryTransferExcept(Inventory recipient, ItemCategory categoriesExcluded, int startAt = 0 ) {
            var success = true;             
            for (int i = startAt; i < Items.Length; ++i)
                if (Items[i] != null && (Items[i].Category & categoriesExcluded) == 0)
                    success = TryTransfer(recipient, i) && success;

            return success;
        }

        public bool IsInventoryFull()
        {
            for (int i = Items.Length -1; i >= 0; i--)
            {
                if (Items[i] == null)
                {
                    return false;
                }
            }
            return true;
        }

        public Inventory GetWorkingCopy()
        {
            Inventory copy = (Inventory)MemberwiseClone();
            copy.Items = new Item[Items.Length];
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null)
                {
                    copy.Items[i] = Items[i].GetWorkingCopy();
                }
            }
            return copy;
        }

        public void Sort()
        {
            Stack();
            var ordered = Items.Where(i => i != null && i.Level > 0).OrderByDescending(i => i.Profile.IsWorkingInInventory && i.Profile.IsActivatable).ThenBy(i => i.Category).
                ThenByDescending( i => i.ID).ToArray();                           
            for (int i = 0; i < Items.Length; i++)
            {
                if(i < ordered.Length)
                {
                    Items[i] = ordered[i];
                }
                else
                {
                    Items[i] = null;
                }
            }
        }

        public int FindItem(string itemID)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null && Items[i].ID == itemID)
                    return i;
            }
            return -1;
        }


        #endregion

        #region Update/Draw
        public void Update(GameEngine gameEngine, Agent agent)
        {            
            for (int i = 0; i < Items.Length; i++)
            {                
                if (Items[i] != null)
                {                   
                    bool tryActivate = false;
                    if (i < QUICK_USE_COUNT)
                    {
                        tryActivate = (quickUseBind[i] & agent.ControlSignal) > 0;                      
                    }
                    
                    Items[i].InventoryUpdate(gameEngine, agent, tryActivate);
                    if (Items[i].Stack <= 0)
                    {
                        Items[i] = null;
                    }                 
                }
                
            }
        }

        public void Draw(Camera camera, Agent agent)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] != null)
                {
                    Items[i].DrawInInventory(camera, agent);
                }
            }
        }

        #endregion
    }
}
