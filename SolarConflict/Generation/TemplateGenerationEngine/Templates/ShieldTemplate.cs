using Microsoft.Xna.Framework;
using SolarConflict.GameContent.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class ShieldTemplate : GenerationTemplate
    {
        

        private readonly string CAPACITY = "Capacity";
        private readonly string GENERATION_RATE_IN_SEC = "GenRateInSec";
        private readonly string DISRUPTED_GENERATION_RATE_IN_SEC = "DisruptedGenRateInSec";
        private readonly string RECHARGE_DELAY = "Recharge delay";

        private readonly string ABILITY = "Ability*";
        private readonly string ABILITY_ACTIVATION = "Ability activation*";
        private readonly string ABILITY_COOLDOWN = "Ability cooldown*";       
        
        private readonly string AURA_COLOR = "Aura Color*";

        protected SlotType itemType;
        protected MeterType MeterType;
        protected MeterType MaxValueMeterType;
        protected string MeterImage;

        public ShieldTemplate():base()
        {
            AddParametereName(ID);                        
            AddParametereName("Size");
            AddParametereName("Price");
            AddParametereName("Level");
            AddParametereName("Icon");

            AddParametereName(CAPACITY);
            AddParametereName(GENERATION_RATE_IN_SEC);
            AddParametereName(DISRUPTED_GENERATION_RATE_IN_SEC);
            AddParametereName(RECHARGE_DELAY);

            AddParametereName(ABILITY);
            AddParametereName(ABILITY_ACTIVATION);
            AddParametereName(ABILITY_COOLDOWN);
            AddParametereName(AURA_COLOR);

            _directoryName = "Shields";
            itemType = SlotType.Shield;
            MeterType = MeterType.Shield;
            MaxValueMeterType = MeterType.ShieldMaxValue;
            MeterImage = "Shield";           
        }

        
        protected override void ParseAndAddEmitter(string[] parameters)
        {
            string id = csvUtils.GetString(ID);
            ShieldData data = new ShieldData(id);
            data.ItemData.IconID = csvUtils.GetString("Icon");
            data.ItemData.BuyPrice = csvUtils.GetInt("Price");
            data.ItemData.Level = csvUtils.GetInt("Level");
            data.ItemData.Size = csvUtils.GetEnum<SizeType>("Size");
            data.Capacity = csvUtils.GetFloat(CAPACITY);
            data.GenerationRatePerSec = csvUtils.GetFloat(GENERATION_RATE_IN_SEC);
            data.InCombatGenerationRatePerSec = csvUtils.GetFloat(DISRUPTED_GENERATION_RATE_IN_SEC, 0);
            data.RechargeDelayInSec = csvUtils.GetFloat(RECHARGE_DELAY, 10);
            //string suffixDescription = string.Format(SUFFIX_DESCRIPTION, maxValue, generationRate * 60, "#image{" + MeterImage + "}");

            data.AbilityEmitterID = csvUtils.GetString(ABILITY);
            if (!string.IsNullOrWhiteSpace(data.AbilityEmitterID))
            {
                data.AbilitiyActivation = csvUtils.GetEnum<ControlSignals>(ABILITY_ACTIVATION, ControlSignals.OnLowHitpoints);
                data.AblitiyCooldown = csvUtils.GetInt(ABILITY_COOLDOWN, 1);
            }
            data.EffectEmitterID = "ShieldAuraFx";
            data.EffectColor = csvUtils.GetColor(AURA_COLOR, Color.Gray);
            Item item = ShieldQuickStart.Make(data);
            item.ID = id;              
            item.ItemFlags |= ItemFlags.Imbuable;
            item.Profile.Category |= ItemCategory.Final;
            item.Profile.Category |= ItemCategory.NonAI;
            ContentBank.Inst.AddContent(item);
        }
    }
}

