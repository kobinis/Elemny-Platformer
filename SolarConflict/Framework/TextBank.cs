using Microsoft.Xna.Framework;
using SolarConflict.Framework.Scenes.DialogEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework
{
    [Serializable]
    public class TextAsset
    {
        public string ID;
        public string Text;
        public String ImageID { get { return GetValue(ParamType.Image); } set { SetValue(ParamType.Image, value); } }
        public String soundID { get { return GetValue(ParamType.Sound); } set { SetValue(ParamType.Sound, value); } }
        public string NextText { get { return GetValue(ParamType.Next); } set { SetValue(ParamType.Next, value); } }

        public TextAsset()
        {
        }

        public TextAsset(string text)
        {
            Text = text;
        }

        public bool? IsBlocking
        {
            get
            {
                string val = GetValue(ParamType.IsBlocking);
                if (string.IsNullOrEmpty(val))
                    return null;
                if (val[0] == 't')
                    return true;
                if (val[0] == 'f')
                    return false;
                return null;              
            }
            set
            {
                if (value == null)
                {
                    SetValue(ParamType.IsBlocking, null);
                }
                else
                {
                    if (value.Value == true)
                        SetValue(ParamType.IsBlocking, "t");
                    else
                        SetValue(ParamType.IsBlocking, "f");
                }
            }
        }
        public bool? IsSkippable
        {
            get
            {
                string val = GetValue(ParamType.IsSkippable);
                if (string.IsNullOrEmpty(val))
                    return null;
                if (val[0] == 't')
                    return true;
                if (val[0] == 'f')
                    return false;
                return null;
            }
            set
            {
                if (value == null)
                {
                    SetValue(ParamType.IsSkippable, null);
                }
                else
                {
                    if (value.Value == true)
                        SetValue(ParamType.IsSkippable, "t");
                    else
                        SetValue(ParamType.IsSkippable, "f");
                }
            }
        }

        Dictionary<ParamType, string> parameters;
        public string GetValue(ParamType paramType)
        {
            return parameters?.Get(paramType);
        }

        public void SetValue(ParamType paramType, string value, bool lenient = true)
        {
            if(parameters == null)
            {
                if (value == null)
                    return;
                parameters = new Dictionary<ParamType, string>();
            }
            Debug.Assert(lenient || !parameters.ContainsKey(paramType), $"Multiply-defined parameter {paramType}");
            if (value != null)
                parameters[paramType] = value;
            else
                parameters.Remove(paramType);
        }
    }

    public class TextBank
    {
        //const int MAX_DIALOG_DEPTH = 100;
        #region Singelton
        private static TextBank bankInstance = null;
        public static TextBank Inst
        {
            get
            {
                if (bankInstance == null)
                {
                    bankInstance = new TextBank();
                }
                return bankInstance;
            }
        }
        #endregion
        Dictionary<string, TextAsset> _bank;
        //Dictionary<string, TextAsset> _tempBank; //TODO:sed to store data for the creation of objects, clearde 

        public TextBank()
        {
            _bank = new Dictionary<string, TextAsset>();
        }

        public IEnumerable<TextAsset> AllAssets => _bank.Values;

        public void Clear()
        {
            _bank.Clear();
        }

        public bool Contains(string ID)
        {
            return _bank.ContainsKey(ID);
        }

        public void AddTextAsset(TextAsset asset)
        {
            if(string.IsNullOrWhiteSpace(asset.ID))
                throw new Exception("TextAsset ID can't be null or empty!");
            if (!_bank.ContainsKey(asset.ID))
            {
                _bank.Add(asset.ID, asset);
            }
            else
            {
                throw new Exception($"TextAsset: {asset.ID} already exists!");
            }
        }

                

        public TextAsset GetTextAsset(string ID)
        {
            if (ID == null)
                return null;
            TextAsset asset;
            if (!_bank.TryGetValue(ID, out asset))
            {                
                if(DebugUtils.Mode == ModeType.Debug)
                    throw new Exception($"TextAsset: {ID} wasn't found!");
                var ta = new TextAsset();
                ta.Text = "Error";
                return ta;
            }
            return asset;
        }

        public TextAsset TryGetTextAsset(string ID)
        {
            if (ID == null)
                return null;
            TextAsset asset;
            _bank.TryGetValue(ID, out asset);
            return asset;
        }

        public Dialog TryGetDialogNode(string ID)
        {

            if (ID != null && _bank.ContainsKey(ID))
                return GetDialogNode(ID);
            return null;
        }

        public void LinkAllAssets()
        {
            foreach (var item in _bank.Values)
            {
                if(item.ID.EndsWith("Start1") || item.ID.EndsWith("End1") || item.ID.EndsWith("Fail1"))
                {
                    TryLinkTextAssets(item.ID.Remove(item.ID.Length - 1));
                }
            }
        }

        public void TryLinkTextAssets(string baseID, int startIndex = 1)
        {
            TextAsset currentAsset = null;
            TextAsset nextAsset = null;
            for (int i = startIndex; ; ++i)
            {
                currentAsset = TryGetTextAsset(baseID + i.ToString());
                nextAsset = TryGetTextAsset(baseID + (i+1).ToString());

                if (currentAsset == null)
                {
                    if(nextAsset != null)
                        ActivityManager.Inst.AddToast(baseID + i.ToString() + " Error at linking", 200, Color.Red);
                    return;
                }

                if (string.IsNullOrWhiteSpace(currentAsset.NextText) && nextAsset != null)
                    currentAsset.NextText = nextAsset.ID;
            }            
        }

        public Dialog GetDialogNode(string ID)
        {                
            return MakeDialogFromAsset(GetTextAsset(ID), ID);
        }

        public string GetString(string ID)
        {
            return GetTextAsset(ID).Text;
        }

        public Dialog MakeDialogFromAsset(TextAsset asset, string ancestorID)// make it a static
        {            
            Dialog dialog = new Dialog(asset.Text, asset.ImageID, asset.IsBlocking);
            dialog.IsSkippable = asset.IsSkippable;
            dialog.ID = ancestorID;
            dialog.SoundID = asset.soundID;
            dialog.TextAssetID = asset.ID;
            if (asset.NextText != null)
            {
                var nextArray = asset.NextText.Split(':');
                if(nextArray.Length == 1)
                    dialog.NextNodeID = asset.NextText;
                else
                {
                    foreach (var item in nextArray)
                    {
                        TextAsset textAsset = TextBank.Inst.GetTextAsset(item);
                        var dialogOption = new DialogOption(textAsset);
                        dialog.AddDialogOption(dialogOption);
                    }
                }
            }
                    //MakeDialogFromAsset(GetTextAsset(asset.NextText), ancestorID, depthCounter +1);
            return dialog;
        }

    }
}
