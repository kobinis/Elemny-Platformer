using Microsoft.Xna.Framework;
using SolarConflict.Framework.GUI;
using SolarConflict.GameContent.Activities;
using SolarConflict.GameContent.Activities.SceneActivitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Session.World.MissionManagment
{
    [Serializable]
    public abstract class MissionObjective
    {
        public enum ObjectiveStatus { Failed = -1, Ongoing = 0, Completed = 1 }
        public string Text { get; set; }
        public bool IsHidden { get; set; }        
        public ObjectiveStatus Status;
        protected List<TutorialGoal> tutorialGoals; //Change from a list to just one

        public MissionObjective()
        {
            Status = ObjectiveStatus.Ongoing;
        }


        public abstract ObjectiveStatus CheckStatus(Mission mission, Scene scene);                  
        public abstract Vector2? GetPosition();
        /// <summary>
        /// The size of the area to get to
        /// </summary>
        public abstract float GetRadius();

        public virtual void Update(Mission mission, Scene scene)
        {
        }

        public virtual string GetObjectiveText()
        {
            if(Text != null)
            {
                return GetStatusTag() + Text;
            }
            return null;
        }

        public virtual  string GetActiveText()
        {
            return null;
        }
       
        public string GetStatusTag()
        {
            return $"#image{{{Status.ToString()}}} ";

        }

        public virtual List<int> GetTargetNodeIndices() {
            return null;
        }

        public virtual List<TutorialGoal> GetTutorialGoals()
        {
            return tutorialGoals;
        }

        public virtual void OnEnterNode() { }

        public void AddTutorialGoal(TutorialGoal goal)
        {
            if (goal == null)
                return;
            if (tutorialGoals == null)
                tutorialGoals = new List<TutorialGoal>();
            tutorialGoals.Add(goal);
        }
    }
}
