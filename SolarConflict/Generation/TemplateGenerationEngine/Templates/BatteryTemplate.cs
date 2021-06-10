using Microsoft.Xna.Framework;
using System;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class BatteryTemplate : ItemGenerationTemplate
    {
        protected readonly string CAPACITY = "Capacity";
        protected readonly string METER_COST = "Meter Cost*";
        protected readonly string METER_TYPE = "Meter Type*";
        protected readonly string ACTIVE_TIME = "Active Time*";

        private readonly string SUFFIX_DESCRIPTION = "\n{0} capacity: {1}{2}";


        public BatteryTemplate()
        {
            _directoryName = "Batteries";
            AddGeneralParameters();
            AddParametereName(CAPACITY);
            AddParametereName("Meter Type");
        }

        protected override Item MakeItem(string ID, string name, string description, int level, SizeType size, string textureID,
                                         string equippedTextureID, float buyPrice, float sellRatio, Color? color)
        {
            float capacity = csvUtils.GetFloat(CAPACITY);
            MeterType meterType = csvUtils.GetEnum<MeterType>("Meter Type");

            string suffixDescription = string.Format(SUFFIX_DESCRIPTION, meterType, capacity, "#Image{" + meterType + "}");

            ItemProfile profile = InitGeneralParameters(ID, name, description + suffixDescription, level, size, textureID, equippedTextureID,
                                                        buyPrice, sellRatio, color, SlotType.Utility);
            profile.SlotType = SlotType.Utility;

            Item item = new Item(profile);
            MeterGenerator system = new MeterGenerator();
            system.MeterType = meterType;
            system.MeterToTakeMaxValue = GetMeterToTakeMaxValue(meterType);
            system.GenerationAmountPerSec = 0;
            system.MaxValue = capacity;
            item.System = system;

            return item;
        }

        private MeterType GetMeterToTakeMaxValue(MeterType MeterType)
        {
            MeterType result;

            switch (MeterType)
            {
                case MeterType.Energy:
                    result = MeterType.EnergyMaxValue;
                    break;
                case MeterType.Shield:
                    result = MeterType.ShieldMaxValue;
                    break;
                default:
                    result = MeterType.EnergyMaxValue;
                    break;
            }

            return result;
        }
    }
}

