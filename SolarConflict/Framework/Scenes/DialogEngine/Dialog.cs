using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework.Scenes.DialogEngine
{
    //[Serializable]
    //public class DialogContext
    //{
    //    public static DialogContext DefaultContext = new DialogContext(null);
    //    public string PortraitID;
    //    public DialogContext(string portraitID)
    //    {
    //        PortraitID = portraitID;
    //    }
    //}

    /// <summary>A dialogue graph</summary>
    [Serializable]
    public class Dialog
    {
        public string ID { get; set; }
        public bool IsFinished { get; set; }
        public string Text { get; set; }        
        public string SoundID { get; set; }
        public string ImageID { get; set; }
        public bool? IsBlocking { get; set; }
        public bool? IsSkippable { get; set; }
        public bool IsInterupting { get; set; }
        public string NextNodeID;
        private List<DialogOption> dialogOptions;

        public string TextAssetID;


        public Dialog(string text, string portraitID = null, bool? isBlocking = true)
        {
            Text = text;
            ImageID = portraitID;
            IsBlocking = isBlocking;       
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            if (TextAssetID != null)
            {
                TextAsset asset = TextBank.Inst.GetTextAsset(TextAssetID);
                Text = asset.Text;
                SoundID = asset.soundID;
                ImageID = asset.ImageID;
                IsBlocking = asset.IsBlocking;
                IsSkippable = asset.IsSkippable;
                if (!string.IsNullOrWhiteSpace(asset.NextText))
                    NextNodeID = asset.NextText;
               // TextBank.Inst.TryLinkTextAssets(TextAssetID.Remove(TextAssetID.Length - 1));
            }
        }


        public Dialog InitCreate(DialogManager dialogManager, Scene scene, Dialog parent)
        {
            if(parent!= null) //And get parent id?
            {
                this.ID = parent.ID;
            }
            //dialogManager.SetDialogBox(Text, ImageID);
            return this;
        }
        
        public void AddDialogOption(DialogOption dialog)
        {
            if(dialogOptions == null)
            {
                dialogOptions = new List<DialogOption>();
            }
            dialogOptions.Add(dialog);
        }                         


        public void Update(DialogManager dialogManager, Scene scene)
        {            
        }

        public Dialog GetNextNode(DialogManager dialogManager, Scene scene)
        {            
            if(NextNodeID != null)
                return TextBank.Inst.GetDialogNode(NextNodeID).InitCreate(dialogManager, scene, this);
            return null;
        }

        public List<DialogOption> GetDialogOptions()
        {
            return dialogOptions;
        }

        public void SelectDialogOption(int index, Scene scene)
        {
            if(dialogOptions[index].NextDialog != null)
                NextNodeID = dialogOptions[index].NextDialog;
            dialogOptions[index].Invoke(this, scene);
            IsFinished = true;
        }
    }
}
