using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using System;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public abstract class MeterGeneratorTemplate : ItemGenerationTemplate
    {

        private readonly string SUFFIX_DESCRIPTION = "\n\nCapacity: {0}{2}\nRegenerate: {1}";


        private readonly string GENERATION_RATE_IN_SEC = "Generation Rate In Seconds";
        private readonly string GENERATION_COOLDOWN = "Generation Cooldown*";
        private readonly int DEFAULT_GENERATION_COOLDOWN = 60; //TODO: maybe remove 
        private readonly string DEPLETION_COOLDOWN = "Depletion Cooldown*";
        private readonly string ABILITY = "Ability*";
        private readonly string ABILITY_ACTIVATION = "Ability activation*";
        private readonly string ABILITY_COOLDOWN = "Ability cooldown*";
        private readonly string RECHARGE_DELAY = "Recharge Delay In Sec";

        


        protected SlotType itemType;
        protected MeterType MeterType;
        protected MeterType MaxValueMeterType;
        protected string MeterImage;

        public MeterGeneratorTemplate()
        {
            AddGeneralParameters();
            AddParametereName(CAPACITY);
            AddParametereName(GENERATION_RATE_IN_SEC);
            AddParametereName(RECHARGE_DELAY);
            AddParametereName(GENERATION_COOLDOWN);
            AddParametereName(DEPLETION_COOLDOWN);
            AddParametereName(ABILITY);
            AddParametereName(ABILITY_ACTIVATION);
            AddParametereName(ABILITY_COOLDOWN);            
        }

        protected override Item MakeItem(string ID, string name, string description, int level, SizeType size, string textureID, 
                                         string equippedTextureID, float buyPrice, float sellRatio, Color? color)
        {
            float maxValue = csvUtils.GetFloat(CAPACITY);
            float generationRate = csvUtils.GetFloat(GENERATION_RATE_IN_SEC) / 60f;

            string suffixDescription = string.Format(SUFFIX_DESCRIPTION, maxValue, generationRate * 60, "#image{" + MeterImage + "}");

            ItemProfile profile = InitGeneralParameters(ID, name, description + suffixDescription, level, size, textureID, equippedTextureID, buyPrice, sellRatio, color, itemType);
            profile.Category = ItemCategory.Generator;

            Item item = new Item(profile);

            MeterGenerator system = new MeterGenerator();
            system.MeterType = MeterType;
            system.MeterToTakeMaxValue = MaxValueMeterType;
            system.MaxValue = maxValue;
            system.GenerationAmountPerSec = generationRate;
            //system.GenerationCooldownTime = csvUtils.GetInt(GENERATION_COOLDOWN, DEFAULT_GENERATION_COOLDOWN); //TODO: add when fixing meter generation
            system.RechargeDelayInFrames = csvUtils.GetInt(DEPLETION_COOLDOWN, 0);
            system.DisruptedGenerationAmountPerSec = csvUtils.GetFloat(RECHARGE_DELAY, 0) / 60f;

            if (string.IsNullOrWhiteSpace(csvUtils.GetString(ABILITY)))
            {
                item.System = system;
            }
            else
            {
                string abilityID = csvUtils.GetString(ABILITY);
                ControlSignals activation = csvUtils.GetEnum<ControlSignals>(ABILITY_ACTIVATION, ControlSignals.OnLowHitpoints);
                int cooldown = csvUtils.GetInt(ABILITY_COOLDOWN, 1);
                EmitterCallerSystem ability = new EmitterCallerSystem(activation, cooldown, abilityID); //maybe sound effect
                SystemGroup systemGroup = new SystemGroup();
                systemGroup.AddSystem(ability, true, false); //Add option to make active shield
                systemGroup.AddSystem(system, false, false); 
                item.System = systemGroup;
            }

            return item;
        }
    }
}
