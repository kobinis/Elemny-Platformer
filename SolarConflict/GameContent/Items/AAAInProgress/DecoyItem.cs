using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.EmitterCallers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.GameContent.Items
{
    class DecoyItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Decoy", "Deploys a decoy", 2, "Light1");
            profile.SlotType = SlotType.Consumable | SlotType.Utility;
            profile.Category |= ItemCategory.Hotbar;
            profile.IsActivatable = true;

            profile.MaxStack = 20;
            profile.IsConsumed = true;
            profile.BuyPrice = 400;
            profile.SellPrice = 250;
            profile.IsWorkingInInventory = true;

            Item item = new Item(profile);
            item.Profile.MaximalRange = 1600;
            item.Profile.SlotType |= SlotType.Utility;

            BasicEmitterCallerSystem gun = new BasicEmitterCallerSystem(ControlSignals.None, "Decoy1");            
            gun.CooldownTime = 30;            
            item.System = gun;
            return item;
        }
    }
}
