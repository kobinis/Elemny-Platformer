using Microsoft.Xna.Framework;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.Scenes.DialogEngine
{
    public delegate void DialogAction(Dialog dialog, Scene scene);
    [Serializable]
    public class DialogOption
    {
        public TextAsset TextAsset { get; set; }
        public string Text => TextAsset.Text;
        public string NextDialog { get; set; }
        public event DialogAction OnSelect;

        public DialogOption(TextAsset textAsset)
        {
            TextAsset = textAsset;
            NextDialog = textAsset.NextText;
        }

        public void Invoke(Dialog dialog, Scene scene)
        {
            //Add open activity
            var activity = TextAsset.GetValue(ParamType.Activity);
            var activityParams = TextAsset.GetValue(ParamType.ActivityParams);
            if (activity != null)
                scene.SwitchActivity(activity, activityParams, callingAgent:scene.DialogAgent);


            // Invoke callback
            OnSelect?.Invoke(dialog, scene);

            var blackboardMessege = TextAsset.GetValue(ParamType.AddToBlackbaordOnSelect);
            if (blackboardMessege != null)
                MetaWorld.Inst.AddToBlackboard(blackboardMessege);


            // Find and invoke other callbacks etc from special params
            var emitterId = TextAsset.GetValue(ParamType.Emitter);
            if (emitterId != null) {
                var emitter = ContentBank.Inst.GetEmitter(emitterId);
                var gameObj = ((GameObject)scene.PlayerAgent) ?? new DummyObject(); 
                emitter?.Emit(scene.GameEngine, null, FactionType.None, gameObj.Position, gameObj.Velocity, gameObj.Rotation);
            }
        }

        //public bool CheckVisibility()
        //{
            
        //}
    }

    //public interface IDialogNode
    //{
    //    string ID { get; set; }        
    //    string Text { get; set; }
    //    string ImageID { get; set; }
    //    string SoundID { get; set; }
    //    bool IsFinished { get; set; }
    //    bool? IsBlocking { get; set; }
    //    bool? IsSkippable { get; set; }
    //    bool IsInterupting { get; set; }
    //    IDialogNode InitCreate(DialogManager dialogManager, Scene scene, IDialogNode parent);
    //    IDialogNode GetNextNode(DialogManager dialogManager, Scene scene);        
    //    void Update(DialogManager dialogManager, Scene scene);
    //    List<DialogOption> GetDialogOptions();
    //    void SelectDialogOption(int index, Scene scene);        
    //}
}
