using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.GUI;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.GameContent.Activities;
using SolarConflict.GameContent.Activities.SceneActivitys;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment
{
    public delegate void MissionAction(Mission mission, Scene scene);

    public enum AgentType { Player, Mothership, Custom }
    public enum MissionState { NonActive, BeforeStartText, InProgress, CompletedBeforeEndText, Done }
    [Serializable]
    public class Mission:IMissionGenerator
    {    
        public string ID { get; set; } //TODO: maybe change to int?
        public AgentType AgentType { get; set; }
        public string Title { get; set; }
        //public string ShortDescription { get; set; }
        public TextAsset Description { get; set; }    
        public TextAsset ActiveTitle { get; set; }
        public TextAsset ActiveDescription { get; set; }
        public string MissionCompleteText { get; set; } 
        public string MissionCompleteSoundEffect { get; set; } //TODO: change to sound emitter
        public StarDate StartMissionDate { get; set; }
        public bool IsSelected { get; set; }
        public bool IsOverrideID { get; set; }
        public FactionType Faction { get; set; }
        //public Agent GivenPlayerObject
        public bool IsTaken { get; set; }
        public bool IsGlobal { get; set; }
        public bool IsFinished { get; set; }       
        public Color Color { get; set; }
        public bool IsDismissable { get; set; }
        public bool IsHidden { get; set; }
        public bool IsGoalHidden { get; set; }
        //public bool IsVisibleInWidget { get; set; }
        public int? DestenationNode { get; set; } //TODO: change
        public Sprite Icon { get; set; }
        public int Lifetime { get; protected set; }
        public Mission NextMissionOnComplete { get; set; }
        public Mission NextMissionOnFail { get; set; }                
        public Dialog DialogOnStart { get; set; }
        public string DialogOnStartID { set { DialogOnStart = TextBank.Inst.GetDialogNode(value); } }
        public Dialog DialogOnComplete { get; set; }
        public string DialogOnCompleteID { set { DialogOnComplete = TextBank.Inst.GetDialogNode(value); } }
        public Dialog DialogOnFail { get; set; }
        public string DialogOnFailID { set { DialogOnFail = TextBank.Inst.GetDialogNode(value); } }
        public IEmitter EmitterOnStart { get; set; }
        public IEmitter EmitterOnComplete { get; set; }
        //public IEmitter EmitterOnFail { get; set; }
        public GameObject MissionGiver;
        public int Level { get; set; }
        public object Data;

        public MissionObjective Objective { get; set; }
        public Agent Agent { get; private set; }

        public event MissionAction OnMissionStart;
        public event MissionAction OnMissionCompletion;
        public event MissionAction OnMissionFailed;

       // public int MoneyReward { get }
        public Reward Reward;

        public MissionState State { get; private set; } //Not in use

        public Mission(string title = null, TextAsset description = null, Sprite Icon = null)
        {                   
            Title = title;
            Description = description;
            Color = Color.CadetBlue;
            IsSelected = true;
            IsOverrideID = true;
            State = MissionState.NonActive;
        }

        [OnDeserialized]
        public void OnDeserializedMethod(StreamingContext context)
        {
            if (Description != null && Description.ID != null)
                Description = TextBank.Inst.GetTextAsset(Description.ID);
        }

        public int? GetNodeIndex()
        {
            return DestenationNode;
        }

        public string GetActiveText()
        {            
            if(ActiveTitle != null)
            {
                StringBuilder sb = new StringBuilder(256);
                sb.AppendLine(Color.ToTag(ActiveTitle.Text));
                sb.Append("#line{}");
                if (ActiveDescription != null)
                    sb.AppendLine(ActiveDescription.Text);
                string objectiveText = Objective.GetActiveText();
                if (objectiveText != null)
                {
                    sb.Append("#line{}");
                    sb.Append(objectiveText);
                }
                //sb.AppendLine();                
                return sb.ToString();
            }
            return null;
        }

        public Vector2? GetPosition()
        {
            if(!IsGoalHidden)
                return Objective?.GetPosition();
            return null;
        }     

        public virtual float GetRadius()
        {
            return Objective.GetRadius();
        }

        public string GetTooltipText() 
        {
            StringBuilder sb = new StringBuilder(256);
            sb.AppendLine(Color.ToTag(Title));
            sb.Append("#line{}");
            if(Description != null)
                sb.AppendLine(Description.Text);
            sb.Append("#line{}");
            sb.Append(Objective.GetObjectiveText());
            sb.AppendLine();
            if (Reward != null)
            {
                sb.Append("#line{}");
                sb.AppendLine(Reward.GetTag());
            }
            else
            {
              
            }
            return sb.ToString();
        }

        /// <summary>
        /// Updates and checks for mission ended (if ended set ISActive to false)
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        public virtual void Update(Scene scene)
        {
            switch (AgentType)
            {
                case AgentType.Player:
                    Agent = scene.PlayerAgent;
                    break;
                case AgentType.Mothership:
                    Agent = scene.GetPlayerFaction().Mothership;
                    break;
                case AgentType.Custom:
                    break;
            }

            Objective.Update(this, scene);           
            var status = Objective.CheckStatus(this, scene);
            switch (status)
            {
                case MissionObjective.ObjectiveStatus.Failed:
                    MissionFailed(scene);
                    break;
                case MissionObjective.ObjectiveStatus.Ongoing:
                    break;
                case MissionObjective.ObjectiveStatus.Completed:
                    MissionCompleted(scene);
                    break;
                default:
                    break;
            }            
            if(Lifetime == 0 && status == MissionObjective.ObjectiveStatus.Ongoing)
            {
                OnMissionFirstUpdate(scene);
            }
            UpdateLogic(status, scene);
            Lifetime++;
        }

        protected virtual void OnMissionFirstUpdate(Scene scene)
        {
            if (DialogOnStart != null)
                scene.DialogManager.AddDialog(DialogOnStart);
            Vector2 pos = Vector2.Zero; //TODO: change
            float rot = 0;
            if(Agent != null)
            {
                pos = Agent.Position;
                rot = Agent.Rotation;
            }
            EmitterOnStart?.Emit(scene.GameEngine, null, Faction, pos, Vector2.Zero, rot, 0, param: scene.GameEngine.Level);
            OnMissionStart?.Invoke(this, scene);
        }

        protected virtual void UpdateLogic(MissionObjective.ObjectiveStatus status, Scene scene)
        {

        }

        private void RemoveStartDialog(Scene scene)
        {
            if(DialogOnStart != null)
            {
                scene.DialogManager.RemoveDialog(DialogOnStart.ID, true);
            }
        }

        protected virtual void MissionFailed(Scene scene)
        {
            RemoveStartDialog(scene);          
            IsFinished = true;            
            OnMissionFailed?.Invoke(this, scene);
            if (DialogOnFail != null)
                scene.DialogManager.AddDialog(DialogOnFail);
            if (NextMissionOnFail != null)
                scene.AddMission(NextMissionOnFail);
        }

        protected virtual void MissionCompleted(Scene scene)
        {
            if(Agent == null && scene.PlayerAgent != null)
            {
                Agent = scene.PlayerAgent;
            }
            if(Agent == null)
            {
                Agent = scene.GetPlayerFaction().Mothership;
            }

            //Reward
            Reward?.AddReward(scene.GameEngine, Faction);
            //scene.GetPlayerFaction().AddValueToMeter(MeterType.Money, MoneyReward);

            RemoveStartDialog(scene);
            if (MissionCompleteSoundEffect != null)
            {
                ActivityManager.Inst.SoundEngine.AddSoundToQue(MissionCompleteSoundEffect, 1); //TODO: change
            }
            if(MissionCompleteText != null)
                ActivityManager.Inst.AddToast(MissionCompleteText, 4*60); //TODO: add time to mission
            IsFinished = true;

            GameObject sourceGameObject = Agent;
            if (sourceGameObject == null)
                sourceGameObject = new DummyObject();
            EmitterOnComplete?.Emit(scene.GameEngine, null, Faction, sourceGameObject.Position, Vector2.Zero, sourceGameObject.Rotation, 0, param: scene.GameEngine.Level);

            if (NextMissionOnComplete != null)
                scene.AddMission(NextMissionOnComplete);
            if (DialogOnComplete != null)
                scene.DialogManager.AddDialog(DialogOnComplete);
            OnMissionCompletion?.Invoke(this, scene);
        }

        //Mission generator
        public Mission GenerateMission()
        {
            return this; //GetWorkingCopy ?? or IsValid = False
        }

        public virtual bool CheckIfValid(Scene scene)
        {
            var status = Objective.CheckStatus(this, scene);
            bool res = status == MissionObjective.ObjectiveStatus.Ongoing;
            if (ID != null)
                res &= !scene.ContainsMissionID(ID);
            return res;
        }

        public List<TutorialGoal> GetTutorialGoals()
        {            
            return Objective.GetTutorialGoals();
        }

        public void AddObjective(MissionObjective objective, bool isNotNeeded = false)
        {
            if (Objective == null)
                Objective = new ObjectiveGroup();
            if(Objective is ObjectiveGroup)
            {
                (Objective as ObjectiveGroup).AddObjective(objective, isNotNeeded);
            }
            else
            {
                throw new Exception("Objective is not an ObjectiveGroup");
            }
            
        }

        
    }
}
