using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class HomeBeacon
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Home Beacon ", "Teleports you to the mother ship", 2,
                    "HomeBeacon", null);
            profile.SlotType = SlotType.Utility;
            profile.IsWorkingInInventory = true;
            profile.IsActivatable = true;
            profile.IsConsumed = false;
            profile.BuyPrice = 3000;
            profile.SellPrice = 1500;
            profile.Category = ItemCategory.Utility;
            //profile.
            Item item = new Item(profile);
            EmitterCallerSystem agentEmitter = new EmitterCallerSystem();
            agentEmitter.CooldownTime = 60;
            agentEmitter.ActivationEmitterID = "TeleportToMotherShip";
            agentEmitter.ActiveTime = 20;
            agentEmitter.EmitterID = "EmitterPickupFx";
            //agentEmitter.EmitterID = 


            item.System = agentEmitter;
            return item;
        }
    }
}
