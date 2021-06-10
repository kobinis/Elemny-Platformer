using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GUI;
using SolarConflict.GameContent.Activities.SceneActivitys;

namespace SolarConflict.Session.World.MissionManagment.Objectives
{ 
    [Serializable]
    public class ObjectiveGroup: MissionObjective
    {
        [Serializable]
        private struct ObjectiveHolder
        {
            public ObjectiveHolder(MissionObjective objective, bool notNeeded, bool requirePreviousObjectives)
            {
                Objective = objective;
                NotNeeded = notNeeded;
                RequirePreviousObjectives = requirePreviousObjectives;
            }
            public MissionObjective Objective;
            public bool NotNeeded;
            public bool RequirePreviousObjectives;
        }

        private List<ObjectiveHolder> _objectives { get; set; }
        private MissionObjective _mainObjective;

        public ObjectiveGroup()
        {
            _objectives = new List<ObjectiveHolder>();
        }

        public void AddObjective(MissionObjective objective, bool notNeeded = false, bool requirePreviousObjectives = false)
        {
            _objectives.Add(new ObjectiveHolder(objective, notNeeded, requirePreviousObjectives));
        }

        public override string GetActiveText()
        {

            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("");
            int i = 0;
            foreach (var item in _objectives)
            {
                if (!item.NotNeeded)
                {
                    if (i > 0)
                        sb.AppendLine();
                    string activeText = item.Objective.GetActiveText();
                    if(activeText != null && !item.NotNeeded)                   
                        sb.Append(item.Objective.GetActiveText());
                    i++;
                }
            }
            if (sb.Length > 0)
                return sb.ToString();
            else
                return null;
        }

        public override string GetObjectiveText()
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("");
            int i = 0;
            foreach (var item in _objectives)
            {
                if (!item.NotNeeded)
                {
                    if(i >0 )
                        sb.AppendLine();
                    sb.Append(item.Objective.GetObjectiveText());
                    i++;
                }
            }
            return sb.ToString();
        }

        public override List<int> GetTargetNodeIndices() {
            var result = new List<int>();
            foreach (var o in _objectives) {
                var subResult = o.Objective.GetTargetNodeIndices();
                if (subResult != null)
                    result.AddRange(subResult);
            }
                
            return base.GetTargetNodeIndices();
        }

        public override Vector2? GetPosition()
        {
            return _mainObjective?.GetPosition();
        }

        public override float GetRadius()
        {
            if (_mainObjective == null)
                return 0;
            return _mainObjective.GetRadius();
        }

        public override void OnEnterNode() {
            foreach (var objective in _objectives)
                objective.Objective.OnEnterNode();
        }

        public override void Update(Mission mission, Scene scene)
        {
            bool isAllCompleted = true;
            foreach (var objective in _objectives)
            {   
                if(!objective.RequirePreviousObjectives || isAllCompleted)
                    objective.Objective.Update(mission, scene);
                if(!objective.NotNeeded && objective.Objective.Status == ObjectiveStatus.Ongoing)
                {
                    isAllCompleted = false;
                }
            }
        }

        public override List<TutorialGoal> GetTutorialGoals()
        {
            List<TutorialGoal> goals = new List<TutorialGoal>();
            if(tutorialGoals != null)
                goals.AddRange(tutorialGoals);
            foreach (var item in _objectives)
            {
                var mgoals= item.Objective.GetTutorialGoals();
                if (mgoals != null)
                    goals.AddRange(mgoals);
            }
            return goals;
        }

        public override ObjectiveStatus CheckStatus(Mission mission, Scene scene)
        {
            _mainObjective = null;
            float distance = float.MaxValue;
            ObjectiveStatus status = ObjectiveStatus.Completed;
            for (int i = 0; i < _objectives.Count; i++)
            {
                var curStatus = _objectives[i].Objective.CheckStatus(mission, scene);
                if(!_objectives[i].NotNeeded)
                    status = (ObjectiveStatus)Math.Min((int)curStatus, (int)status);
                if(_objectives[i].Objective.GetPosition().HasValue && scene.PlayerAgent != null && curStatus == ObjectiveStatus.Ongoing)
                {
                    float curDistance = GameObject.DistanceFromEdge(_objectives[i].Objective.GetPosition().Value, scene.PlayerAgent.Position, _objectives[i].Objective.GetRadius(), scene.PlayerAgent.Size);
                    if (curDistance < distance)
                    {
                        _mainObjective = _objectives[i].Objective;
                        distance = curDistance;
                    }
                }                            
            }
            return status;
        }
    }
}
