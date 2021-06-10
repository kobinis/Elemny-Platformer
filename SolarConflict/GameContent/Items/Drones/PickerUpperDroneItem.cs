using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items
{
    class PickerUpperDroneItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemQuickStart.Profile("Emergency Salvage Drone", "Collects equipment dropped when your\nship is destroyed and returns it to the mothership", 3,
                "RepairDroneItem", null);
            profile.SlotType = SlotType.Utility | SlotType.Consumable;            
            profile.MaxStack = 5;
            profile.IsConsumed = true;
            profile.BuyPrice = ScalingUtils.ScaleCost(profile.Level);
            profile.SellPrice = 2500;

            Item item = new Item(profile);
            EmitterCallerSystem gun = new EmitterCallerSystem(ControlSignals.OnDestroyed, "PickerUpperDrone");
            gun.CooldownTime = 30;
            gun.EmitterSpeed = 10;
            item.System = gun;

            return item;
        }
    }
}
