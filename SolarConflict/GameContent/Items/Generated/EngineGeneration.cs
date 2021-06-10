using SolarConflict.GameContent.Utils;
using SolarConflict.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.Items.Generated
{
    class EngineGeneration
    {
        public static IEmitter Make()
        {
            for (int i = 0; i < ScalingUtils.NumLevels; i++)
            {
                SizeType size = SizeType.Small;
                if (i > 5)
                    size = SizeType.Medium;                
                ContentBank.Inst.AddContent(MakeItem(i, (SizeType)size));
            }
                //for (int size = 0; size <= (int)SizeType.Medium; size++)
                //{
                   
                //}            
            return null;
        }

        private static Item MakeItem(int level, SizeType size)
        {
            var icons = new string[] { "engine0", "engine4sdf", "engine2b" };

            EngineData data = new EngineData($"{size} Engine {level}");
            data.ItemData.Size = size;
           
            data.ItemData.IconID = icons[(int)size];

            if (level > 0)
                data.ItemData.SecounderyIconID = $"lvl{level}";

            if (size == SizeType.Medium)
                data.ItemData.Category |= ItemCategory.NonAI;
            
            // data.ItemData.EquippedTextureId = "ShipEngine";

            data.ItemData.BreaksClocking = false;

            data.MaxSpeed = ScalingUtils.ScaleEngineSpeed(level);
            data.Force = (2f + 1.5f * level) * (((int)size) + 1);

            data.ItemData.BuyPrice = (int)Math.Round(ScalingUtils.ScaleCost(level) * 0.3f);
            data.ItemData.Level = level;
            var item = EngineQuickStart.Make(data);
            item.ID = $"{size}Engine{level}";
            item.Profile.BreaksCloaking = false;
            return item;
        }

    }
}
