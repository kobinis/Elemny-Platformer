using SolarConflict.GameContent.Projectiles;
using SolarConflict.Generation;

namespace SolarConflict.GameContent.Items
{
    class RepairDroneLauncherItem
    {
        public static Item Make()
        {
            ItemProfile profile = ItemCommon.CommonProfile();
            profile.Level = 4;
            profile.BuyPrice = ScalingUtils.ScaleCost(profile.Level);
            profile.Name = "Repair Drone";
            profile.DescriptionText = "It's a drone that repair you!";
            profile.IconTextureID = "RepairDroneItem";
            profile.IsConsumed = false;            
            profile.IsActivatable = true;
            profile.MaxStack = 1;
            profile.SlotType = SlotType.Utility;
            profile.Level = 2; 

            Item item = new Item(profile);
            EmitterCallerSystem emitter = new EmitterCallerSystem(ControlSignals.OnLowHitpoints, 60, typeof(RepairDrone).Name);
            emitter.ActivationCheck.AddItemCost("RepairDroneItem", 1);
            item.System = emitter;

            return item;
        }
    }
}
