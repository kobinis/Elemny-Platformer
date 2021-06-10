using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.GameContent.Items
{
    class BoomerangAmmoItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Boomerang", "Boomerang gun ammo.", 1, "boomerang", null);
            profile.SlotType = SlotType.Ammo;
            profile.ItemSize = SizeType.Small;            
            profile.IsActivatable = false;
            profile.BuyPrice = 50;
            profile.SellPrice = 30;
            profile.MaxStack = 999;
            profile.Category |=  ItemCategory.Boomerang;
            profile.AmmoEmitter = ContentBank.Inst.GetEmitter("BoomerangShot");            
            Item item = new Item(profile);
            return item;
        }     
    }
}
