using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.ImbeItems
{
    class LifelineImbue
    {
        public static Item Make()
        {
            ItemData itemData = new ItemData("Lifeline Component", 3, "LifeLineRS");
            itemData.Category |= ItemCategory.Imbuing;
            itemData.SlotType = SlotType.Consumable;
            itemData.BuyPrice = 1500;
            Item item = ItemQuickStart.Make(itemData);
            string ability = "Restores hitpoints and shield";
            string trigger = "Low hitpoints";
            string cooldown = "60 Sec";
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
            imbueSystem.PrefixText = "Lifeline";
            imbueSystem.DescriptionText = text;

            EmitterCallerSystem system = new EmitterCallerSystem("EmitterPickupFx");
            system.Color = Color.LightGreen;
            system.ActivationCheck = new ActivationCheck(ControlSignals.OnLowHitpoints);
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.AddEntry(MeterType.Hitpoints, 2000);
            system.SelfImpactSpec.AddEntry(MeterType.Shield, 5000);
            system.CooldownTime = 60 * 60;
            imbueSystem.system = system;
            return imbueSystem;
        }
    }
}
