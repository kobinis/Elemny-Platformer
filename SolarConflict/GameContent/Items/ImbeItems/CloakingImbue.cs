﻿using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.ImbeItems
{
    class CloakingImbue
    {
        public static Item Make()
        {
            ItemData itemData = new ItemData("Cloaking Component", 2, "CloackingRS");
            itemData.Category |= ItemCategory.Imbuing;
            itemData.SlotType = SlotType.Consumable;
            itemData.BuyPrice = 1000;
            Item item = ItemQuickStart.Make(itemData);
            string ability = "Cloaking";
            //string trigger = "A";
            string cooldown = "30 Sec";
            StringBuilder sb = new StringBuilder();
            sb.Append("#line{}");
            sb.AppendLine(Palette.Highlight.ToTag("Ability: ") + ability);
           // sb.AppendLine(Palette.Highlight.ToTag("Trigger: ") + trigger);
            sb.Append(Palette.Highlight.ToTag("Cooldown: ") + cooldown);
            string prefixText = sb.ToString();

            item.Profile.DescriptionText = "Used to imbue a shield or a generator with a passive ability\n";
            item.Profile.DescriptionText += prefixText;
            item.System = MakeSystem(prefixText);
            item.Profile.Category |= ItemCategory.Final;
            return item;
        }

        public static ImbueSystem MakeSystem(string descriptionText)
        {
            ImbueSystem imbueSystem = new ImbueSystem();
            imbueSystem.PrefixText = "Cloaking";
            imbueSystem.DescriptionText = descriptionText;

            CloakingSystem system = new CloakingSystem();
            system.ActivationEmitterID = "FxEmitterCloak";
            system.Cooldown = 60 * 30;
            
            imbueSystem.system = system;
            return imbueSystem;
        }
    }
}
