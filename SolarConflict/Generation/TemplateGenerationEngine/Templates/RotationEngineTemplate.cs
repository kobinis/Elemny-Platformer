using Microsoft.Xna.Framework;
using System;

namespace SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates
{
    public class RotationEngineTemplate : ItemGenerationTemplate
    {
        private readonly string SUFFIX_DESCRIPTION = "\n\nRotation force: {0}";
        private readonly string FORCE = "Force";

        public RotationEngineTemplate()
        {
            _directoryName = "RotationEngines";
            AddGeneralParameters();
            AddParametereName(FORCE);
        }

        protected override Item MakeItem(string ID, string name, string description, int level, SizeType size, string textureID,
                                         string equippedTextureID, float buyPrice, float sellRatio, Color? color)
        {
            float rotationForce = csvUtils.GetFloat(FORCE);

            string suffixDescription = string.Format(SUFFIX_DESCRIPTION, rotationForce);

            ItemProfile profile = InitGeneralParameters(ID, name, description + suffixDescription, level, size, textureID, equippedTextureID, buyPrice, sellRatio, color, SlotType.Rotation);
            profile.Category = ItemCategory.Rotation;
            profile.IsActivatable = true;

            Item item = new Item(profile);
            item.System = new AgentRotationEngine(rotationForce);
            item.Profile.Category |= ItemCategory.Final;
            return item;
        }
    }
}




