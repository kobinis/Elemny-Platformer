using Microsoft.Xna.Framework;
using System;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class RawMaterialsTemplate : NonEquippedItemsTemplate
    {
        private readonly int DEFAULT_MAX_STACK = 999;

        public RawMaterialsTemplate()
        {
            _directoryName = "RawMaterials";
            AddParametereName(MAX_STACK);
            itemType = SlotType.None;
        }

        protected override Item MakeItem(string ID, string name, string description, int level, SizeType size, string textureID, float buyPrice, float sellRatio, Color? color)
        {
                var item = base.MakeItem(ID, name, description, level, size, textureID, buyPrice, sellRatio, color);
                item.Profile.MaxStack = csvUtils.GetInt(MAX_STACK, DEFAULT_MAX_STACK);
                item.Profile.Category |= ItemCategory.Material;
                return item;
        }
    }
}

