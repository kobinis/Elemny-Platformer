using SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine;
using SolarConflict.GameContent.ContentGeneration.TemplateGenerationEngine.Templates;
using SolarConflict.GameContent.Items;
using SolarConflict.Generation.TemplateGenerationEngine.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.GameContent.ContentGeneration
{   
    public class TemplateGenerationManager
    {
        private string contentDirectory = SolarConflict.Framework.Consts.TEMPLATES_PATH;

        List<GenerationTemplate> generationTemplates;        
        public TemplateGenerationManager()
        {
            generationTemplates = new List<GenerationTemplate>();       
        }

        public void Add(GenerationTemplate template)
        {
            generationTemplates.Add(template);
        }

        public void GenerateContent()
        {
            foreach (var template in generationTemplates)
            {
                template.LoadDirectory(contentDirectory);
            }
        }
    }
}
