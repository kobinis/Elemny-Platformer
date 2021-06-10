using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject
{
    [Serializable]
    public struct InventoryItemEntery
    {
        public InventoryItemEntery(Item item, int amount):this()
        {
         
            this.Item = item;
            this.Amount = amount;
        }

        [JsonIgnore]
        public Item Item { get; private set; }

        public string ItemId
        {
            get
            {
                if (Item == null)
                {
                    return null;
                }

                return Item.ID;
            }
            set
            {
                Item = ContentBank.Inst.GetItem(value,false);
            }
        }

        public int Amount { get; set; }
    }

}
