using Microsoft.Xna.Framework;
using System;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public abstract class NonEquippedItemsTemplate : ItemGenerationTemplate
    {
        protected readonly string MAX_STACK = "Max Stack*";
        protected readonly string COOLDOWN = "Cooldown*";
        protected readonly int DEFAULT_COOLDOWN = 60;
        private readonly int DEFAULT_MAX_STACK = 200;

        protected SlotType itemType;
        protected ItemCategory category;
        protected int MaxStack;
        protected bool IsRetainedOnDeath = false;

        public NonEquippedItemsTemplate()
        {
            AddGeneralParameters();
            RemoveParametereName(EQUIPPED_TEXTURE);
            MaxStack = DEFAULT_MAX_STACK;
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            Item item = MakeItem(csvUtils.GetString(ID), csvUtils.GetString(NAME), csvUtils.GetString(DESCRIPTION), csvUtils.GetInt(QUALITY),
                                 csvUtils.GetEnum<SizeType>(SIZE, DEFAULT_SIZE), csvUtils.GetString(TEXTURE), csvUtils.GetInt(PRICE),
                                 csvUtils.GetFloat(SELL_RATIO), csvUtils.GetColor(COLOR));
            
            ContentBank.Inst.AddContent(item);
        }

        protected virtual Item MakeItem(string ID, string name, string description, int level, SizeType size, string textureID, float buyPrice, float sellRatio, Color? color)
        {            
            ItemProfile profile = InitGeneralParameters(ID, name, description, level, size, textureID, null, buyPrice, sellRatio, color, itemType);
            //profile.BackgroundTextureProxy = new XnaUtils.Graphics.Sprite("glow128");
            profile.MaxStack = MaxStack;
            profile.IsRetainedOnDeath = IsRetainedOnDeath;
            profile.Category = category;

            Item item = new Item(profile);

            return item;
        }      
    }
}
