using SolarConflict.Framework;
using SolarConflict.Framework.GUI;
using SolarConflict.GameContent.Activities;
using SolarConflict.GameContent.Activities.SceneActivitys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SolarConflict.Session.World.MissionManagment
{
    [Serializable]
    public class MissionManager
    {
        public bool IsGlobal = false;
        public bool IsAutoSelectMission { get; set; }
        public MissionGeneratorsBank GeneratorBank { get; private set; }
        private Dictionary<string, Mission> missionsDic;
        private List<Mission> missionList { get { return missionsDic.Values.ToList(); } }
        private uint counter = 0;


        public MissionManager(bool isGlobal)
        {
            IsGlobal = isGlobal;
            GeneratorBank = new MissionGeneratorsBank();
            missionsDic = new Dictionary<string, Mission>();
        }

        public string GetNewMissionID()
        {
            counter++;
            return counter.ToString() + "_";
        }

        public void Clear()
        {
            missionsDic.Clear();
        }

        public bool AddMission(Mission mission)
        {
            mission.IsTaken = true;
            if (mission.ID == null)
            {
                counter++;
                mission.ID = counter.ToString() + "_";
            }
            
            if (mission.IsOverrideID)
            {
                missionsDic[mission.ID] = mission;
                return true;
            }
            else
            {
                if (!missionsDic.ContainsKey(mission.ID))
                {
                    missionsDic.Add(mission.ID,mission); //Check for dupicate ID's???
                    return true;
                }
            }
            return false;
        }
        
        public void RemoveMission(Mission mission)
        {
            missionsDic.Remove(mission.ID);
            if(!mission.IsDismissable)
            {
                MetaWorld.Inst.CodexManager.AddMission(mission);
            }
        }

        public Mission GetMission(string id)
        {
            Mission resMission;
            missionsDic.TryGetValue(id, out resMission);            
            return resMission;
        }

        public bool ContainsMissionID(string id)
        {
            return missionsDic.ContainsKey(id);
        }

        public List<Mission> GetMissions()
        {
            return missionList;
        }

        public List<Mission> GetSelectetMissions()
        {
            List<Mission> selectedMissions = new List<Mission>();
            foreach (var mission in missionList)
            {
                Debug.Assert(IsGlobal == mission.IsGlobal);
                if (mission.IsSelected)
                    selectedMissions.Add(mission);
            }

            return selectedMissions;
        }

        public void OnEnterNode() {
            foreach (var mission in missionList)
                mission.Objective.OnEnterNode();
        }
        
        public void Update(Scene scene)
        {
            var missions = missionList;
            bool isOneMissionSelected = false;
            for (int i = missions.Count - 1; i >= 0; i--)
            {
                Mission mission = missions[i];
                isOneMissionSelected |= mission.IsSelected;
                mission.Update(scene);
                if(mission.IsFinished)
                {
                    RemoveMission(mission);
                }
            }
            if (IsAutoSelectMission && !isOneMissionSelected && missions.Count > 0)
                missions[0].IsSelected = true;
        }

        public List<TutorialGoal> GetTutorialGoals()
        {
            List<TutorialGoal> goals = new List<TutorialGoal>();           
            foreach (var mission in missionList)
            {
                var missionGoals = mission.GetTutorialGoals();
                if(mission.IsSelected && missionGoals != null )
                {
                    foreach (var goal in missionGoals)
                    {
                        if (goal != null)
                            goals.Add(goal);
                    }                    
                }
            }
            return goals;
        }
    }
}
