using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.NewItems.Utils
{
    class SpaceBrake
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Space Brake I", "Uses the fabric of spaces to slow down"
            , 1, "salvage-drone");
            profile.SlotType = SlotType.Utility;            
            profile.IsActivatable = true;
            profile.BuyPrice = 800;
            profile.SellPrice = 500;
            profile.MaxStack = 1;

            Item item = new Item(profile);

            EmitterCallerSystem system = new EmitterCallerSystem();
            system.ActivationCheck = new ActivationCheck(ControlSignals.None);
            //system.EmitterID = "EmitterPickupFx";
            system.CooldownTime = 1;
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.ForceType = ForceType.Mult;
            system.SelfImpactSpec.Force = 0.1f;            
            item.System = system;
            item.Profile.Level = 11;
            return item;
        }
    }
}
