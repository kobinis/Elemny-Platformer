using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Generated
{
    class CloakingDeviceGeneration
    {
        public static IEmitter Make()
        {
            //var step = 2;
            //for (int i = 3; i < ScalingUtils.NumLevels; i += step)
            var step = 1;
            for (int i = 1; i < ScalingUtils.NumLevels; i += step)
                ContentBank.Inst.AddContent(MakeCloakingItem(i));

            return null;
        }

        public static Item MakeCloakingItem(int level)
        {
            float fromShieldScaling = 0.8f;
            CloakingDeviceData data = new CloakingDeviceData($"Cloacking {level}");
            data.ItemData.IconID = "cloakingdevice1";
            if (level > 0)
                data.ItemData.SecounderyIconID = $"lvl{level}";

            data.ItemData.Category = ItemCategory.Cloaking;
            data.ItemData.Level = level;
            data.ItemData.BuyPrice = ScalingUtils.ScaleCost(level);
            data.Capacity = ScalingUtils.ScaleShieldCapacity(level) * fromShieldScaling;
            data.GenerationRate = ScalingUtils.ScaleShieldGenerationPerFrame(level) * fromShieldScaling;
            data.ReCloakingCooldown = Math.Max(30 - level * 5,4) * 30;
            data.ItemData.Size = SizeType.Small;
            if (level > 5)
                data.ItemData.Size = SizeType.Medium;

            Item item = CloakingDeviceQuickStart.Make(data);
            item.ItemFlags |= ItemFlags.Imbuable;
            item.ID = $"Cloack{level}";
            item.Profile.Level = Math.Max(item.Profile.Level, 3);

            return item;
        }
    }
}
