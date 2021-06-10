using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class DevastationCore
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Devastation Core", "Ammo for #color{255,165,0}Devastation Core Detonator#color{255,255,255}", 3, "DevastationCore", null);
            profile.SlotType = SlotType.Ammo;
            profile.Category = ItemCategory.Core;
            profile.ItemSize = SizeType.Large;            
            profile.IsActivatable = false;
            profile.BuyPrice = 150;
            profile.SellPrice = 50;
            profile.MaxStack = 999;            
            Item item = new Item(profile);
            item.Profile.AmmoEmitter = ContentBank.Inst.GetEmitter("DevastationShot");
            return item;
        }     
    }
}
