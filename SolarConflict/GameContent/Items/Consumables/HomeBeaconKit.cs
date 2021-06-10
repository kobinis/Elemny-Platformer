using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class HomeBeaconKit
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Consumable Home Beacon", Palette.Highlight2.ToTag("Teleports you to the mothership")+"\nConsumed on use", 1,
                    "HomeBeaconKit", null);
            profile.SlotType = SlotType.Consumable;            
            profile.IsActivatable = true;
            profile.IsConsumed = true;
            profile.IsShownOnHUD = true;            
            profile.BuyPrice = 300;
            profile.SellPrice = 200;
            profile.MaxStack = 10;
            profile.Category = ItemCategory.Consumable | ItemCategory.Hotbar;
            profile.IsWorkingInInventory = true;


            //profile.
            Item item = new Item(profile);
            EmitterCallerSystem agentEmitter = new EmitterCallerSystem();
            agentEmitter.CooldownTime = 60 * 5;
            agentEmitter.ActivationEmitterID = "TeleportToMotherShip";
            agentEmitter.ActiveTime = 20;
            agentEmitter.EmitterID = "EmitterPickupFx";
            agentEmitter.SelfImpactSpec = new CollisionSpec();
            agentEmitter.SelfImpactSpec.AddEntry(MeterType.StunTime, 5);
            item.System = agentEmitter;
            return item;
        }
    }
}
