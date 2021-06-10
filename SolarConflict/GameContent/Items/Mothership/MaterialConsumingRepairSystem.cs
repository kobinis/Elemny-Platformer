using SolarConflict.GameContent.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Mothership
{
    class MaterialConsumingRepairSystem
    {
        public static Item Make()
        {
            ItemData data = new ItemData("Repair System", 1, "crafting-kit");
            data.BuyPrice = 1000;
            data.SlotType = SlotType.Utility;
            data.Size = SizeType.Large;
            EmitterCallerSystem system = new EmitterCallerSystem(ControlSignals.OnHitpointsNotFull, "EmitterPickupFx");
            system.ActivationCheck.AddItemCost("MatA1", 3);
            system.CooldownTime = 60 * 3;
            system.SelfImpactSpec = new CollisionSpec();
            system.SelfImpactSpec.AddEntry(MeterType.Hitpoints, 300);
            Item item = ItemQuickStart.Make(data);
            item.System = system;
            return item;
        }
    }
}
