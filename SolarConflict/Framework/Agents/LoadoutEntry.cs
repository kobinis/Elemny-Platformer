using Newtonsoft.Json;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject
{
    [Serializable]
    public struct LoadoutEntry  // TODO: maybe change to private inner struct for agent loadout
    {
        public ControlSignals Activation { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
        public int LocationID { get; set; }

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
                if (value == null)
                    return;
                if (ContentBank.Inst.ContainsEmitter(value) || value[0] == '#')
                {
                    Item = ContentBank.Inst.GetItem(value, false);
                }
                else
                {         
                               
                   // throw new Exception("Iems:" + value + " not found");
                }
            }
        }

        public LoadoutEntry(Item item, ControlSignals activation, int locationID): this()
        {
            this.Item = item;
            this.Activation = activation;
            this.LocationID = locationID;
        }
    }
}
