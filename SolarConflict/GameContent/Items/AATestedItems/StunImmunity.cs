using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.AATestedItems
{
    class StunImmunity
    {
        public static Item Make()
        {
            //, "Removes stun"
            ItemData data = new ItemData("EMP Protection", 6, "item_shield_1_a");
            data.BuyPrice = 10000;            
            data.Category = ItemCategory.Utility;
            data.SlotType = SlotType.Utility;
            var item = ItemQuickStart.Make(data);
            item.Profile.IsActivatable = false;
            var system = new EmitterCallerSystem(ControlSignals.OnStun,0,"EmitterPickupFx");
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.AddEntry(MeterType.StunTime, 0, ImpactType.Min);
            item.System = system;
            return item;
        }
    }
}
