using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Generation;
using SolarConflict.Session.World.MissionManagment;
using SolarConflict.Session.World.MissionManagment.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    /// <summary>
    /// System that holds MissionGenerators spesific to that agent
    /// </summary>
    [Serializable]
    public class MissionBankSystem : AgentSystem
    {
        public List<IMissionGenerator> MissionList { get; private set; }
        private List<IMissionFactory> _missionFactories;
        public int MaxMissionNum { get; set; }

        public MissionBankSystem()
        {
            MissionList = new List<IMissionGenerator>();
            MaxMissionNum = 10;
        }

        public List<IMissionGenerator> GetMissions(Scene scene, Agent calling_agent)
        {
            //check for list in the mission manager
            Dictionary<string, IMissionGenerator> missionDic = new Dictionary<string, IMissionGenerator>();
            foreach (var item in scene.GetMissions())
            {
                missionDic[item.ID] = item;
            }
            var removeList = new List<IMissionGenerator>(MissionList.Count);
            foreach (var mission in MissionList)
            {
                if (mission.IsTaken || !mission.CheckIfValid(scene))
                    removeList.Add(mission);
                else
                {
                    if(missionDic.ContainsKey(mission.ID))
                    {
                        removeList.Add(mission);
                    }
                    else
                    {
                        missionDic.Add(mission.ID, mission);
                    }
                }

            }
            foreach (var item in removeList)
            {
                MissionList.Remove(item);
            }

            

            if (_missionFactories != null && MissionList.Count < MaxMissionNum)
            {
                var newMissions = new List<IMissionGenerator>();
                foreach (var factory in _missionFactories)
                {
                    factory.CrateMissionGenerator(scene, calling_agent, null, newMissions);
                }
                newMissions.Shuffle(scene.GameEngine.Rand);
                foreach (var newMission in newMissions)
                {
                    if(!missionDic.ContainsKey(newMission.ID))
                    {
                        missionDic.Add(newMission.ID, newMission);
                        MissionList.Add(newMission);
                        if (MissionList.Count >= MaxMissionNum)
                            return MissionList;
                    }
                }
            }

            return MissionList;
        } 

        public void AddMissionFactory(IMissionFactory missionFactory)
        {
            if (_missionFactories == null)
                _missionFactories = new List<IMissionFactory>();
            _missionFactories.Add(missionFactory);
        }

        public override AgentSystem GetWorkingCopy() //TODO: check
        {
            MissionBankSystem system = new MissionBankSystem();
            if(_missionFactories != null)
            {
                system._missionFactories = new List<IMissionFactory>(_missionFactories);
            }
            foreach (var mission in MissionList)
            {
                system.MissionList.Add(mission);
            }
            return system;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            return false;
        }
    }
}