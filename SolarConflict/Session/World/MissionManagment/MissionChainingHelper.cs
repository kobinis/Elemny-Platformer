using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Session.World.MissionManagment
{
    class MissionChainingHelper
    {
        enum AddType { Both, Compleate, Falil}

        private Scene scene;
        public Mission LastMissionAdded;
        //private Mission pr
        public MissionChainingHelper(Scene scene)
        {
            this.scene = scene;
        }

        public void AddGroup(bool chainFirstMission, params Mission[] missions) //Remove
        {
            if (LastMissionAdded == null)
            {
                foreach (var mission in missions)
                {
                    scene.AddMission(mission);
                }                
            }
            else
            {       
                NextMissionGroup group = new NextMissionGroup(missions);
                LastMissionAdded.OnMissionCompletion += group.AddMissionsHandler;
                LastMissionAdded.OnMissionFailed += group.AddMissionsHandler;
                if (chainFirstMission)
                {
                    LastMissionAdded.NextMissionOnComplete = missions[0]; //TODO: depending on the type
                    LastMissionAdded.NextMissionOnFail = missions[0];
                    LastMissionAdded = missions[0];
                }
            }            
        }

        public void Add(Mission mission)
        {
            if(LastMissionAdded == null)
            {
                scene.AddMission(mission);
            }
            else
            {
                LastMissionAdded.NextMissionOnComplete = mission; //TODO: depending on the type
                LastMissionAdded.NextMissionOnFail = mission;
            }
            LastMissionAdded = mission;
        }  
        
        //MakeDelayMission
        //AddWithDelay
    }

    [Serializable]
    public class NextMissionGroup
    {
        List<Mission> missions;

        public NextMissionGroup(IEnumerable<Mission> inMissions)
        {
            missions = new List<Mission>(inMissions);
        }

        public void AddMissionsHandler(Mission mission, Scene scene)
        {
            foreach (var m in missions)
            {
                scene.AddMission(m);
            }
        }
    }
}
