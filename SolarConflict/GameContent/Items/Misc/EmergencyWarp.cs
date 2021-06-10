using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Misc {
    class EmergencyWarp {
        public static Item Make() {
            var profile = ItemQuickStart.Profile("Emergency Warp Beacon", "Deploys a subspace anchor in a system, warps the fleet to that system when critically damaged.\nBypasses warp inhibitors.",
                1, "FactionDockingBay", "item3");
            profile.SlotType = SlotType.Mothership;
            profile.ItemSize = SizeType.Large;
            profile.Category = ItemCategory.Mothership;
            profile.BuyPrice = 3000;
            profile.SellPrice = 1700;
            profile.IsActivatable = false;
            profile.IsShownOnHUD = true;

            var system = new SystemGroup();

           system.AddSystem(     new EmergencyWarpSystem());

            EmitterCallerSystem kit = new EmitterCallerSystem();
            kit.EmitterID = "EmitterPickupFx"; //TODO: change to green sparks
            kit.ActivationCheck.controlMask = ControlSignals.OnLowHitpoints;
            kit.CooldownTime = 60 * 60; ;
            kit.SelfImpactSpec = new CollisionSpec();
            kit.SelfImpactSpec.AddEntry(MeterType.Hitpoints, 3000);
            kit.SelfImpactSpec.AddEntry(MeterType.Shield, 2000);
            system.AddSystem(kit);

            var item = new Item(profile);
            item.Profile.IsActivatable = true;
            item.Profile.IsConsumed = false;
            item.System = system;
            item.Profile.SlotType = SlotType.Utility;
            
            return item;

        }
    }
}
