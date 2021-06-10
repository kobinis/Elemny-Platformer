using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.ImbeItems
{
    class PushImbue
    {
        public static Item Make()
        {
            ItemData itemData = new ItemData("Shockwave Component", 1, "PushRS");
            itemData.Category |= ItemCategory.Imbuing;
            itemData.SlotType = SlotType.Consumable;
            itemData.BuyPrice = 500;
            Item item = ItemQuickStart.Make(itemData);
            string ability = "Push nearby objects";
            string trigger = "Receiving damage";
            string cooldown = "30 Sec";
            StringBuilder sb = new StringBuilder();
            sb.Append("#line{}");
            sb.AppendLine(Palette.Highlight.ToTag("Ability: ") + ability);
            sb.AppendLine(Palette.Highlight.ToTag("Trigger: ") + trigger);
            sb.Append(Palette.Highlight.ToTag("Cooldown: ") + cooldown);
            string prefixText = sb.ToString();

            item.Profile.DescriptionText = "Used to imbue a shield or a generator with a passive ability\n";
            item.Profile.DescriptionText += prefixText;
            item.System = MakeSystem(prefixText);
            return item;
        }

        public static ImbueSystem MakeSystem(string text)
        {
            ImbueSystem imbueSystem = new ImbueSystem();
            imbueSystem.PrefixText = "Shockwave";
            imbueSystem.DescriptionText = text;

            EmitterCallerSystem system = new EmitterCallerSystem("AoePush1");
            system.MaxLifetime = 90;
            system.ActivationCheck = new ActivationCheck(ControlSignals.OnTakingDamage);
            //system.Color = Color;
            system.SelfImpactSpec = new CollisionSpec();
            system.CooldownTime = 60 * 30;

            imbueSystem.system = system;
            return imbueSystem;
        }
    }
}
