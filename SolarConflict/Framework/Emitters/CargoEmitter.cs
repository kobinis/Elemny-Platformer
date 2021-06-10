using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;

namespace SolarConflict
{
    [Serializable]
    public class InventoryEmitter : IEmitter
    {        
        public string ID { get; set; }        
        public ItemCategory ItemCategory;
        public bool EmmitAll;

        public InventoryEmitter(ItemCategory itemCategory = ItemCategory.All, bool emmitAll = false)
        {
            this.ItemCategory = itemCategory;
            EmmitAll = emmitAll;
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
          int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            Inventory cargo = parent.GetInventory();
            if (cargo != null)
            {                
                for (int i = 0; i < cargo.Size; i++) //TODO: change it, you don't need to go over all the Capacity
                {
                    Item item = cargo.GetItem(i);
                    if (item != null && (item.Profile.Category & ItemCategory) > 0)
                    {
                        Item cargoItem = cargo.RemoveItem(i);
                        if (cargoItem != null)
                        {                           
                            cargoItem.Parent = parent;
                            // Refresh cargo lifetime on emission
                            cargoItem.Lifetime = 0;                           
                            cargoItem.Position = refPosition;
                            cargoItem.Velocity = refVelocity;
                            cargoItem.Rotation = refRotation;
                            cargoItem.RotationSpeed = refRotationSpeed;
                            gameEngine.AddList.Add(cargoItem);
                            if (!EmmitAll)
                            {
                                break;
                            }
                        }
                        
                    }
                }
            }
            return null;
        }


        
    }
}
