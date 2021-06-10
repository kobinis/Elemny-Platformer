//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using SolarConflict.Framework.Utils;
//using SolarConflict.GameContent.Items;
//using SolarConflict.Framework.Agents.Systems;

//namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
//{
//    public class ShopTemplate : ItemGenerationTemplate
//    {
//        private readonly string PERSON_TEXTURE = "Person Texture";
//        private readonly string SHOP_LEVEL = "Shop Level*";
//        private readonly string WELCOME_TEXT = "Welcome Text*";
//        private readonly string INVENTORIES = "Inventories*";


//        private readonly int DEFAULT_SHOP_LEVEL = 1;
//        private readonly FactionType DEFAULT_FACTION = FactionType.Neutral;

//        public ShopTemplate()
//        {
//            _directoryName = "Shops";
//            AddParametereName(ID);
//            AddParametereName(NAME);
//            AddParametereName(PERSON_TEXTURE);
//            AddParametereName(SHOP_LEVEL);
//            AddParametereName(WELCOME_TEXT);   // TODO: use this field
//            AddParametereName(INVENTORIES);  
//        }

//        protected override void ParseAndAddEmitter(string[] parameters)
//        {
//            ShopSystem shop = new ShopSystem();
//         //   shop.ID = csvUtils.GetString(ID);
//      ///      shop.ShopName = csvUtils.GetString(NAME, shop.ID);
//            shop.SellerImageID = csvUtils.GetString(PERSON_TEXTURE);
//         // if  shop.ShopLevel = csvUtils.GetInt(SHOP_LEVEL, DEFAULT_SHOP_LEVEL);
//            AddItemsFromInventories(shop);

//            AddItems(parameters, shop);

//            ItemProfile profile = ItemQuickStart.Profile(shop.ShopName, "put it in a slot to make it a shop", 0, "ShieldItem", "ShieldItem");
//            profile.SlotType = SlotType.Utility;
//            profile.IsWorkingInInventory = true;
//            profile.IsRetainedOnDeath = true;
//            profile.IsActivatable = true;

//            Item item = new Item(profile);
//           // item.ID = shop.ID;
//            item.System = shop;

//            ContentBank.Inst.AddContent(item);
//        }

//        private static void AddItems(string[] parameters, ShopSystem shop)
//        {
//            for (int i = 6; i < parameters.Length; i++)
//            {
//                if (parameters[i] != string.Empty)
//                {
//                    string[] itemData = parameters[i].Split(':');

//                    if (itemData.Length == 1)
//                    {
//                        shop.AddItem(itemData[0]);
//                    }
//                    if (itemData.Length > 1)
//                    {
//                        shop.AddItem(itemData[0], Parser.ParseFloat(itemData[1], 1));
//                    }
//                }
//            }
//        }

//        private void AddItemsFromInventories(ShopSystem shop)
//        {
//            string inventories = csvUtils.GetString(INVENTORIES);

//            if (!string.IsNullOrEmpty(inventories))
//            {
//                string[] inventoriesData = inventories.Split(':');

//                foreach (string inventoryID in inventoriesData)
//                {

//                }
//            }
//        }
//    }
//}

