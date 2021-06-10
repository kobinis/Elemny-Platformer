using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    public class Recipe : IEmitter //TODO: move recipe out
    {
        [Serializable]
        public struct CraftingCost
        {
            public CraftingCost(string itemId, int amount = 1, bool isConsumed = true)
            {
                this.ItemNeeded = ContentBank.Inst.GetItem(itemId, false).Profile;
                this.Amount = amount;
                this.IsConsumed = isConsumed;
            }

            public ItemProfile ItemNeeded;
            public int Amount;
            public bool IsConsumed;
        }

        public string ID
        {
            get { return CraftedItem.ID + "Recipe"; }
            set { throw new Exception("No need to set ID"); }
        }

        public Item CraftedItem { get; private set; }
        public int AmountRecived { get; private set; }
        public List<CraftingCost> CraftingCostList { get; private set; } 
        public CraftingStationType CraftingStation { get; private set; }        
        //float moneyCost = 0; //???

        public Recipe(string craftedItemId, CraftingStationType craftingStation = CraftingStationType.Basic, int amountRecived = 1)
        {
            CraftedItem = ContentBank.Inst.GetItem(craftedItemId,false);
            CraftingStation = craftingStation;
            AmountRecived = amountRecived;
            CraftingCostList = new List<CraftingCost>();
        }

        public void AddCost(string itemId, int amount = 1, bool isConsumed = true)
        {
            if (amount > 0)
            {
                CraftingCostList.Add(new CraftingCost(itemId, amount, isConsumed));
            }
        }

        public bool IsCraftable(IEnumerable<Inventory> inventorys)
        {
            Dictionary<string, int> itemCount = new Dictionary<string, int>();
            Inventory.CountItems(inventorys, itemCount);
            return IsCraftable(itemCount); 
        }

        public bool IsCraftable(Dictionary<string, int> itemCount) 
        {
            foreach (var itemCost in CraftingCostList)
            {
                int numInInventory;
                itemCount.TryGetValue(itemCost.ItemNeeded.Id, out numInInventory);
                if (numInInventory < itemCost.Amount)
                    return false;
            }
            return true;
        }

        public Item Craft(GameEngine gameEngine, IEnumerable<Inventory> inventoryList, bool skipCraftable = false)
        {
            
            if(skipCraftable || IsCraftable(inventoryList))
            {

                Dictionary<string, int> totalCost = new Dictionary<string, int>();                
                foreach (var cost in CraftingCostList)
                {
                    if (cost.IsConsumed)
                    {
                        totalCost.Add(cost.ItemNeeded.Id, cost.Amount);
                    }
                }

                foreach (var inventory in inventoryList)
                {
                    if (inventory == null)
                        continue;
                    for (int i = 0; i < inventory.Size; i++)
                    {
                        Item invItem = inventory.GetItem(i);
                        if (invItem != null && invItem.ID != null)
                        {
                            int amountToRemove;
                            if(totalCost.TryGetValue(invItem.ID, out amountToRemove))
                            {
                                int minRemoveAmount = Math.Min(amountToRemove, invItem.Stack);
                                invItem.Stack -= minRemoveAmount;
                                totalCost[invItem.ID] -= minRemoveAmount;
                                if(minRemoveAmount == amountToRemove)
                                {
                                    totalCost.Remove(invItem.ID);
                                    if (totalCost.Count == 0)
                                        break;
                                }
                            }
                        }
                    }
                }

                Item item = (Item)CraftedItem.MakeGameObject(gameEngine);
                item.Stack = AmountRecived;
                return item;
            }
            return null;
        }

        public bool IsCraftable(Inventory inventory)
        {
            foreach (var itemCost in CraftingCostList)
            {
                if (!inventory?.CheckForItem(itemCost.ItemNeeded.Id, itemCost.Amount) == true)
                    return false;
            }
            return true;
        }

        public GameObject Craft(GameEngine gameEngine, Inventory inventory)
        {
            //?Possibly check for room after drain, if no room return cost

            ////check if there is room in the inventory?
            //if (!inventory.HasRoom(CraftedItem.ID, AmountRecived))
            //    return null; //maybe it will return the index

            //check if inventory got the needed items
            foreach (var itemCost in CraftingCostList)
            {
                if (!inventory.CheckForItem(itemCost.ItemNeeded.Id, itemCost.Amount))
                    return null;
            }

            //drain Cost
            foreach (var itemCost in CraftingCostList)
            {
                if (itemCost.IsConsumed)
                    inventory.RemoveItem(itemCost.ItemNeeded.Id, itemCost.Amount);
            }

            Item item = (Item)CraftedItem.MakeGameObject(gameEngine); //??
            item.Stack = AmountRecived; //??

            return item;
        }

        public bool CraftToInventory(GameEngine gameEngine, Inventory inventory) //split function//Fix
        {   
            //?Possibly check for room after drain, if no room return cost

            //check if there is room in the inventory?
            if (!inventory.HasRoom(CraftedItem.ID, AmountRecived))
                return false; //maybe it will return the index

            //check if inventory got the needed items
            foreach (var itemCost in CraftingCostList)
            {
                if (!inventory.CheckForItem(itemCost.ItemNeeded.Id, itemCost.Amount))
                    return false;
            }

            //drain Cost
            foreach (var itemCost in CraftingCostList)
            {
                if (itemCost.IsConsumed)
                    inventory.RemoveItem(itemCost.ItemNeeded.Id, itemCost.Amount);
            }

            Item item = (Item)CraftedItem.MakeGameObject(gameEngine); //??
            item.Stack = AmountRecived; //??

            inventory.AddItem(item);

            return true;
        }   

       

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
           int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Inventory cargo = parent.GetInventory();

            if (cargo != null)
            {
                GameObject item = Craft(gameEngine, cargo);

                if (item != null)
                {
                    item.Parent = parent;
                    item.Position = refPosition;
                    item.Velocity = refVelocity;
                    item.Rotation = refRotation;
                    item.RotationSpeed = refRotationSpeed;
                    gameEngine.AddList.Add(item);
                }

                return item;
            }

            return null;
        }

        public string GetRecipeText(Dictionary<string, int> itemsAvailable = null, bool showCraftingStation = false)
        {
            StringBuilder sb = new StringBuilder(256);
            if (this.AmountRecived > 1)
            {
                sb.Append(this.AmountRecived.ToString() + " x ");
            }
            sb.AppendLine(CraftedItem.Tag);

            sb.Append("#line{}");
            if (CraftingCostList.Count > 0)
            {
                
                foreach (var itemCost in CraftingCostList)
                {
                    if (itemsAvailable != null)
                    {
                        var color = (itemsAvailable.Get(itemCost.ItemNeeded.Id) >= itemCost.Amount) ? Color.Green : Color.Gray;
                        sb.Append(color.ToTag());
                    }
                    sb.Append(itemCost.Amount);
                    sb.Append(" x ");
                    if (itemsAvailable != null)
                        sb.Append("#dcolor{}");
                    sb.Append("#image{" + itemCost.ItemNeeded.IconTextureID + "} ");
                    sb.Append(itemCost.ItemNeeded.GetColor().ToTag(itemCost.ItemNeeded.Name));
                    sb.AppendLine();
                }                
            }
            if (showCraftingStation && CraftingStation != CraftingStationType.Uncraftable)
            {
                sb.AppendLine("Crafted at: #hcolor{}" + this.CraftingStation.GetUserName() + "#dcolor{}");
                sb.AppendLine("#s{48}"+this.CraftingStation.GetIconTag());
            }

            sb.Append(CraftedItem.GetTooltipText(false, true, false, false));            
            return sb.ToString();
        }      

    }
}
