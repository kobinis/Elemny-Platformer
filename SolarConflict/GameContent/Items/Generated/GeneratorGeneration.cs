
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using SolarConflict.Framework.Utils;
using SolarConflict.GameContent.ContentGeneration.Items;
using SolarConflict.GameContent.Emitters;
using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using SolarConflict.NewContent.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Generated
{
    class GeneratorGeneration
    {
        public static IEmitter Make()
        {
            for (int i = 0; i < ScalingUtils.NumLevels; i++)
                ContentBank.Inst.AddContent(MakeGeneratorItem(i));

            return null;
        }

        public static Item MakeGeneratorItem(int level)
        {
            GeneratorData data = new GeneratorData($"Generator {level}");
            data.ItemData.IconID = "GeneratorItem2";
            if (level > 0)
                data.ItemData.SecounderyIconID = $"lvl{level}";

            data.ItemData.Category = ItemCategory.Shield;
            data.ItemData.Level = level;
            data.ItemData.BuyPrice = ScalingUtils.ScaleCost(level);
            data.Capacity = ScalingUtils.ScaleEnergyCapacity(level);
            data.GenerationRatePerSec = ScalingUtils.ScaleEnergyGenerationPerFrame(level) * 60;
            data.InCombatGenerationRatePerSec = data.GenerationRatePerSec;
            Item item = GeneratorQuickStart.Make(data);
            item.ItemFlags |= ItemFlags.Imbuable;
            item.ID = $"Generator{level}";
            if (level == 1)
                item.Profile.Level = 2;
            return item;
        }



    }
}
