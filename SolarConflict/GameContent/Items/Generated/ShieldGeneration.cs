
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
    class ShieldGeneration
    {
        public static IEmitter Make()
        {
            for (int i = 1; i < ScalingUtils.NumLevels; i++)
                ContentBank.Inst.AddContent(MakeShieldItem(i));

            return null;
        }

        public static Item MakeShieldItem(int level)
        {
            ShieldData data = new ShieldData($"Shield {level}");
            data.ItemData.IconID = "ShieldIcon1";
            if (level > 0)
                data.ItemData.SecounderyIconID = $"lvl{level}";
            if (level > 5)
                data.ItemData.Size = SizeType.Medium;
            
            data.ItemData.Category = ItemCategory.Shield;
            data.ItemData.Level = level;
            data.ItemData.BuyPrice = ScalingUtils.ScaleCost(level, 100);
            data.RechargeDelayInSec = 15;
            data.Capacity = ScalingUtils.ScaleShieldCapacity(level);
            data.GenerationRatePerSec = ScalingUtils.ScaleShieldGenerationPerFrame(level) * 60;
            data.InCombatGenerationRatePerSec = 0;
            
            Item item = ShieldQuickStart.Make(data);
            item.ItemFlags |= ItemFlags.Imbuable;
            item.ID = $"Shield{level}";
            if (level <= 2)
            {
                item.Profile.Category |= ItemCategory.NonAI;
                item.Profile.Level = 3;
            }

            return item;
        }

       
       
    }
}
