using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Utils;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    // KOBI: should we add shop inventory
    public class ShopInventoryTemplate : ItemGenerationTemplate
    {
        public ShopInventoryTemplate()
        {
            _directoryName = "ShopsInventories";
            AddParametereName(ID);
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            //ShopSystem shop = new ShopSystem();
            //shop.ID = csvUtils.GetString(ID);

            //for (int i = 1; i < parameters.Length; i++)
            //{
            //    if (parameters[i] != string.Empty)
            //    {
            //        string[] itemData = parameters[i].Split(':');

            //        if (itemData.Length == 1)
            //        {
            //            shop.AddItemEntry(itemData[0]);
            //        }
            //        if (itemData.Length > 1)
            //        {
            //            shop.AddItemEntry(itemData[0], Parser.ParseInt(itemData[1], 1));
            //        }
            //    }
            //}

            //ContentBank.Inst.AddContent(shop);
        }
    }
}

