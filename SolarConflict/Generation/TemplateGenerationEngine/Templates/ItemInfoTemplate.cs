using Microsoft.Xna.Framework;
using SolarConflict.GameContent.ContentGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Generation.TemplateGenerationEngine.Templates
{
    /// <summary>
    /// Adds name and description to items
    /// </summary>
    class ItemInfoTemplate: GenerationTemplate
    {
        const string FLAVOUR = "Flavour*";
        const string PARAMS = "Params*";
        const string NAME = "Name*";
        const string DESCRIPTION = "Description";

        const string COMMENTS = "Comments*";

        public ItemInfoTemplate()
        {
            _directoryName = "ItemInfoTemplate";
            AddParametereName(ID);
            AddParametereName(NAME);
            AddParametereName(DESCRIPTION);
            AddParametereName(FLAVOUR);
            AddParametereName(COMMENTS);
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            string id = csvUtils.GetString(ID);
            if (!string.IsNullOrWhiteSpace(id))
            {
                try
                {
                    Item item = ContentBank.Inst.GetItem(id, false);                    
                    item.Profile.Name = csvUtils.GetString(NAME, item.Name);                    
                    string description = csvUtils.GetString(DESCRIPTION);                                        
                    if(!string.IsNullOrWhiteSpace(description))
                        item.Profile.DescriptionText = description;
                    var falvour = csvUtils.GetString(FLAVOUR);
                    if (!string.IsNullOrWhiteSpace(falvour))
                        item.Profile.FlavourText = falvour;
                }
                catch (Exception)
                {
                    //Write to log
                }

              
            }
        }
    }
}
