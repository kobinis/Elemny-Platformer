using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.GameContent.ContentGeneration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Generation.TemplateGenerationEngine.Templates
{
    class TextAssetsTemplate: GenerationTemplate
    {
        const string TEXT = "Text";
        const string IMAGE = "Image*";
        const string SOUND = "Sound*";
        const string NEXT_TEXT = "NextText*";
        const string IS_BLOCKING = "IsBlocking*";
        const string IS_SKIPPABLE = "IsSkippable*";
        const string PARAMS = "Params*";
        const string BLACKBAORD_NEED = "Blackbaord Need*";
        const string ADD_BLACKBOARD = "Add Blackbaord*";
        const string ACTIVITY = "Activity*";
        const string ACTIVITY_PARAMS = "ActivityParams*";
        const string COMMENTS = "Comments*";

        public TextAssetsTemplate()
        {
            _directoryName = "TextAssets";
            AddParametereName(ID);
            AddParametereName(TEXT);
            AddParametereName(IMAGE);          
            AddParametereName(NEXT_TEXT);
            AddParametereName(IS_BLOCKING);
            AddParametereName(IS_SKIPPABLE);
            AddParametereName(PARAMS);
            AddParametereName(BLACKBAORD_NEED);
            AddParametereName(ADD_BLACKBOARD);
            AddParametereName(ACTIVITY);
            AddParametereName(ACTIVITY_PARAMS);
            AddParametereName(COMMENTS);
            AddParametereName(SOUND);

        }

        protected override void ParseAndAddEmitter(string[] parameters)
        {
            string id = "??";
            TextAsset asset = new TextAsset();
            try
            {                     
                id = csvUtils.GetString(ID);
                if (!string.IsNullOrWhiteSpace(id) && !id.StartsWith("#"))
                {
                    
                    asset.ID = id;
                    asset.Text = csvUtils.GetString(TEXT);
                    asset.ImageID = csvUtils.GetString(IMAGE);
                    asset.NextText = csvUtils.GetString(NEXT_TEXT);
                    if (asset.NextText == id)
                        throw new Exception("NextText can't be equal to ID:" + id);
                    if (!string.IsNullOrWhiteSpace(csvUtils.GetString(SOUND)))
                    {
                        asset.soundID = csvUtils.GetString(SOUND);
                    }
                    if (!string.IsNullOrWhiteSpace(csvUtils.GetString(IS_BLOCKING)))
                    {
                        asset.IsBlocking = csvUtils.GetBool(IS_BLOCKING);
                    }
                    if (!string.IsNullOrWhiteSpace(csvUtils.GetString(IS_SKIPPABLE)))
                    {
                        asset.IsSkippable = csvUtils.GetBool(IS_SKIPPABLE);
                    }

                    if (!string.IsNullOrWhiteSpace(csvUtils.GetString(ACTIVITY)))
                    {
                        asset.SetValue(ParamType.Activity,csvUtils.GetString(ACTIVITY));
                    }

                    if (!string.IsNullOrWhiteSpace(csvUtils.GetString(ACTIVITY_PARAMS)))
                    {
                        asset.SetValue(ParamType.ActivityParams, csvUtils.GetString(ACTIVITY_PARAMS));
                    }

                    if (!string.IsNullOrWhiteSpace(csvUtils.GetString(PARAMS)))
                    {
                        //(
                        var specialParams = csvUtils.GetString(PARAMS).Split(',').Select(s => s.Trim()).ToArray();
                        foreach (var s in specialParams)
                        {
                            var pair = s.Split(new[] { ':' }, 2);
                            Debug.Assert(pair.Length == 2, "Special dialogue parameter wasn't a colon-separated pair");
                            var type = ParserUtils.ParseEnum<ParamType>(pair[0]);
                            asset.SetValue(type, pair[1], false);
                        }
                    }
                    TextBank.Inst.AddTextAsset(asset);
                }
                          
            }
            catch (Exception e)
            {

                ActivityManager.Inst.AddToast("Text Asset Error at: " +id +"\n" + e.ToString(), 10 * 60, Color.Red);
            }

            if (DebugUtils.Mode == ModeType.Debug && DebugUtils.IsReload)
            {
                if (asset.Text != null)
                {

                    try
                    {
                        Game1.font.DefaultCharacter = null;
                        Game1.font.MeasureString(asset.Text);
                    }
                    catch (Exception)
                    {
                        Game1.font.DefaultCharacter = '*';
                        //ActivityManager.Inst.AddToast("Error at: " + asset.id, 200, Color.Red);
                        throw new Exception("TextAsset ID: " + asset.ID + "\nText: " + asset.Text);
                    }
                    finally
                    {
                        Game1.font.DefaultCharacter = '*';
                    }

                }
            }

        }
    }
}
