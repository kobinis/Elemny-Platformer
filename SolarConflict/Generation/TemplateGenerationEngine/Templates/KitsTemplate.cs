using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class KitsTemplate : NonEquippedItemsTemplate
    {
        private readonly string DEFAULT_EFFECT_NAME = "EmitterPickupFx";
        private readonly int DEFAULT_MAX_STACK = 20;
        private readonly string SUFFIX_DESCRIPTION = "\nIncrease {0} {1}{2}";

        public KitsTemplate()
            : base()
        {
            _directoryName = "Kits";
            AddParametereName(MAX_STACK);
            AddParametereName("Control Signal");
            AddParametereName(COOLDOWN);
            AddParametereName("Effect Name*");
            AddParametereName("Meter Type");
            AddParametereName("Meter Amount");
        }

        protected override Item MakeItem(string ID, string name, string description, int level, SizeType size, string textureID, float buyPrice, float sellRatio, Color? color)
        {
            MeterType meterType = csvUtils.GetEnum<MeterType>("Meter Type");
            float meterAmount = csvUtils.GetInt("Meter Amount");
            string suffixDescription = string.Format(SUFFIX_DESCRIPTION, meterAmount, meterType, "#Image{" + meterType + "}");

            ItemProfile profile = InitGeneralParameters(ID, name, description + suffixDescription, level, size, textureID, null, buyPrice, sellRatio, color, SlotType.None);
            profile.MaxStack = csvUtils.GetInt(MAX_STACK, DEFAULT_MAX_STACK);
            profile.IsRetainedOnDeath = false;
            profile.IsActivatable = true;
            profile.IsWorkingInInventory = true;
            profile.IsConsumed = true;

            Item item = new Item(profile);

            string effectName = csvUtils.GetString("Effect Name*", DEFAULT_EFFECT_NAME);

            EmitterCallerSystem kit = new EmitterCallerSystem(csvUtils.GetEnum<ControlSignals>("Control Signal"), csvUtils.GetInt(COOLDOWN, DEFAULT_COOLDOWN), effectName);
            kit.ActivationCheck.AddCost(MeterType.GlobalRepairCooldown, ShipCommon.repairCooldownTime);
            kit.SelfImpactSpec = new CollisionSpec();
            kit.SelfImpactSpec.AddEntry(meterType, meterAmount);
            kit.SelfImpactSpec.AddEntry(MeterType.GlobalRepairCooldown, 0, ImpactType.Min);
            item.System = kit;

            return item;
        }

    }
}
