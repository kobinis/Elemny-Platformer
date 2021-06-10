using Microsoft.Xna.Framework;
using SolarConflict.GameContent.ContentGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Generation.TemplateGenerationEngine.Templates
{
    class PaletteTemplate : GenerationTemplate
    {
        private readonly string STYLE1 = "Style1*";

        public PaletteTemplate()
        {
            _directoryName = "Palette";
            AddParametereName(ID);
            AddParametereName(STYLE1);
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            /*

            MyObject obj = new MyObject();
obj.GetType().InvokeMember("Name",
    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
    Type.DefaultBinder, obj, "Value");
            */
            string id = csvUtils.GetString(ID);
            if (!string.IsNullOrWhiteSpace(id))
            {
                Color? color = csvUtils.GetColor(STYLE1);
                if (color != null)
                {
                    Color colorValue = color.Value;
                    var type = typeof(Palette);
                    var field = type.GetField(id);
                    field.SetValue(null, colorValue);
                }
            }
        }


    }
}


