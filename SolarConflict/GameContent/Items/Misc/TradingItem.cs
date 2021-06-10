//using SolarConflict.GameContent.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SolarConflict.GameContent.Items
//{
//    public class TradingItem
//    {
//        public static Item Make()
//        {
//            float sellRatio = 0.5f;
//            float cooldown = 2;
//            ItemData data = new ItemData("Trading System");
//            data.Description = "Sells raw materials for " +sellRatio.ToString()+" of their sell cost\n";
//            data.Description += "Cooldown: " + cooldown.ToString();
//            data.IconID = "replace";
//            data.EquippedTextureId = null; 
//            data.Level = 0;            
//            ItemProfile profile = ItemQuickStart.Profile(data);
//            Item item = new Item(profile);
//            item.Profile.SlotType = SlotType.Utility;
//            TradingSystem system = new TradingSystem();
//            system.CooldownTime = (int)(cooldown * 60);
//            system.SellRatio = sellRatio;
//            item.System = system;
//            return item;
//        }
//    }
//}
