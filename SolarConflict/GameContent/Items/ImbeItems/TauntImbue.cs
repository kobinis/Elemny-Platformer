using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems.Misc;
using SolarConflict.Framework.Emitters;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items.ImbeItems
{
    class TauntImbue
    {
        public static Item Make()
        {
            ItemData itemData = new ItemData("Taunt Component", 5, "SlowdownRS"); //TODO: change icon
            itemData.Category |= ItemCategory.Imbuing;
            itemData.SlotType = SlotType.Consumable;
            itemData.BuyPrice = 1500;
            Item item = ItemQuickStart.Make(itemData);
            string ability = "Taunt";
            string trigger = "On Stun";
            string cooldown = "60 Sec";
            StringBuilder sb = new StringBuilder();
            sb.Append("#line{}");
            sb.AppendLine(Palette.Highlight.ToTag("Ability: ") + ability);
          //  sb.AppendLine(Palette.Highlight.ToTag("Trigger: ") + trigger);
         //   sb.Append(Palette.Highlight.ToTag("Cooldown: ") + cooldown);
            string prefixText = sb.ToString();

            item.Profile.DescriptionText = "Used to imbue a shield or a generator with a passive ability\n";
            item.Profile.DescriptionText += prefixText;
            item.System = MakeSystem(prefixText);
            return item;
        }

        public static ImbueSystem MakeSystem(string text)
        {
            ImbueSystem imbueSystem = new ImbueSystem();
            imbueSystem.PrefixText = "Taunt";
            imbueSystem.DescriptionText = text;

            var emitter = new TauntEmitter(3500);

            BasicEmitterCallerSystem system = new BasicEmitterCallerSystem(ControlSignals.AlwaysOn, emitter);
            system.CooldownTime = 60 * 2;            
            imbueSystem.system = system;
            return imbueSystem;
        }
    }
}
