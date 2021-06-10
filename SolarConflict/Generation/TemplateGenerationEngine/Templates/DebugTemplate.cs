using Microsoft.Xna.Framework;
using SolarConflict.GameContent.ContentGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Generation.TemplateGenerationEngine.Templates
{
    class DebugTemplate : GenerationTemplate
    {

        public DebugTemplate()
        {
            _directoryName = "DebugSettings";
            AddParametereName(ID);
            AddParametereName(ModeType.Release.ToString());
            AddParametereName(ModeType.Debug.ToString());
            AddParametereName(ModeType.Test.ToString());
        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {

            string readFrom = DebugUtils.Mode.ToString();

            string id = csvUtils.GetString(ID);
            if (!string.IsNullOrWhiteSpace(id))
            {
                bool value = csvUtils.GetBool(readFrom, false);                
                var type = typeof(DebugUtils);
                var field = type.GetField(id);
                field.SetValue(null, value);                
            }
        }


    }
}


