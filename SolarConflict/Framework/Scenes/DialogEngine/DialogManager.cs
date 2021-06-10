using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework;
using SolarConflict.Framework.Scenes.DialogEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.SimpleGui;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Session.World.MissionManagment
{
    [Serializable]
    public class DialogManager
    {
        public const string DialogVolumeID = "DialogVolume";
        public GuiManager gui { get; private set; }
        private Queue<Dialog> dialogQueue;
        private Dialog currentDialog;
        private DialogBoxControl dialogBox;
        private Scene scene;
        [NonSerialized]
        private SoundEffectInstance soundEffect;
        
        //private DialogContext Context;

        public DialogManager(Scene scene)
        {
            this.scene = scene;
            dialogQueue = new Queue<Dialog>();
            gui = new GuiManager();            
        }

        public Dialog AddDialogBox(string text, string portraitID, bool isBlocking = false, bool isSkippable = true)
        {
            Dialog dialog = new Dialog(text, portraitID, isBlocking);
            dialog.IsSkippable = isSkippable;
            AddDialog(dialog);
            return dialog;
        }

        public void AddDialog(string dialogID)
        {
            AddDialog(TextBank.Inst.GetDialogNode(dialogID));            
        }

        public void AddDialog(Dialog dialog) 
        {
            if(dialog != null)
                dialogQueue.Enqueue(dialog);
        }

        private void SetDialog(Dialog dialog)
        {
            if (dialog == null)
                return;
            bool isBlocking = dialog.IsBlocking ?? false;
            bool isSkippable = dialog.IsSkippable ?? true;
            var dialogOptions = dialog.GetDialogOptions();           
            soundEffect?.Stop();
            if(dialog.SoundID != null && AudioBank.Inst.GetSoundDictionary().ContainsKey(dialog.SoundID))
            {
                soundEffect = AudioBank.Inst.GetSound(dialog.SoundID).CreateInstance(); //Play a beep if not found
                soundEffect.Volume = VolumeSettings.DialogVolume;
                soundEffect.Play();
            }
            dialogBox = new DialogBoxControl(this, dialog.Text, dialog.ImageID, isSkippable: isSkippable, isBlocking: isBlocking, dialogOptions: dialogOptions);
        }

        public void Update()
        {
            gui.Update(scene.InputState); 
            while (dialogQueue.Count > 0 && (currentDialog == null || dialogQueue.Peek().IsInterupting)  )  //TODO: change or ture to intruptable          
            {                
                var nextDialog = dialogQueue.Dequeue();
                if (!nextDialog.IsFinished)
                {
                    currentDialog = nextDialog.InitCreate(this, scene, null);
                    SetDialog(currentDialog);
                }
            }
            scene.PauseGameEngine = false;
            if (currentDialog != null)
            {
                if(dialogBox != null && dialogBox.IsBlocking && currentDialog != null && !currentDialog.IsFinished)
                    scene.PauseGameEngine = true;
                currentDialog.Update(this,scene);
                if (currentDialog.IsFinished)
                {
                    dialogBox = null;
                    currentDialog = currentDialog.GetNextNode(this,scene);
                    SetDialog(currentDialog);
                }
            }
            if(currentDialog == null)
            {              
                dialogBox = null;
            }
            
            
            if(dialogBox != null)
            {
                dialogBox.Update(scene.InputState);
                dialogBox.Position = new Vector2(ActivityManager.ScreenSize.X * 0.5f, ActivityManager.ScreenSize.Y - dialogBox.HalfSize.Y - 40);
                if(currentDialog.IsFinished)
                {                   
                    dialogBox = null;
                }
            }

            
        }

        //public void SetDialogBox(string text, string portraitID)
        //{
        //    dialogBox = new DialogBoxControl(this, text, portraitID, isSkippable: true);
        //}

        public bool RemoveDialog(string id, bool isHardRemove) 
        {
            if(currentDialog!= null && currentDialog.ID == id)
            {
                if(isHardRemove)
                    currentDialog = null;
                else
                    currentDialog.IsFinished = true;
                
                return true;
            }
            foreach (var dialog in dialogQueue)
            {
                if(dialog.ID == id)
                {
                    dialog.IsFinished = true;
                    return true;
                }
            }
            return false;
        }
       

        public void ContinieHandler() //TODO: make private and pass functions to dialog box
        {
            if (currentDialog != null)
                currentDialog.IsFinished = true;
        }

        public void DialogOptiponSelected(int index)
        {
            if (currentDialog != null)
            {
                currentDialog.SelectDialogOption(index, scene);
            }
        }

        public void Draw()
        {

            Game1.sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            dialogBox?.Draw(scene.Camera.SpriteBatch, null);
            Game1.sb.End();
            gui.Draw(); //This can get controllers thet follow on screen objects
        }

        public void StopDialog()
        {
            soundEffect?.Stop();
        }
    }
}
