using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.ImbeItems
{
    class EmpImbue
    {
        public static Item Make()
        {
            ItemData itemData = new ItemData("EMP Component", 4, "EmpRS");
            itemData.Category |= ItemCategory.Imbuing;
            itemData.SlotType = SlotType.Consumable;
            itemData.BuyPrice = 1500;
            Item item = ItemQuickStart.Make(itemData);

            string ability = "Stun nearby enemies for 8 sec";
            string trigger = "Low energy";
            string cooldown = "45 Sec";
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
            imbueSystem.PrefixText = "EMP";
            imbueSystem.DescriptionText = text;

            EmitterCallerSystem system = new EmitterCallerSystem("AoeStun2");
            system.MaxLifetime = 90;
            system.ActivationCheck = new ActivationCheck(ControlSignals.OnLowEnergy);
            //system.Color = Color;
            system.SelfImpactSpec = new CollisionSpec();
            system.CooldownTime = 60 * 45;

            imbueSystem.system = system;
            return imbueSystem;
        }
    }
}
