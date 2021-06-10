//using Microsoft.Xna.Framework;
//using SolarConflict.Framework;
//using SolarConflict.Framework.Agents;
//using SolarConflict.Framework.Agents.Systems;
//using SolarConflict.Generation;
//using SolarConflict.Session.World.MissionManagment;
//using SolarConflict.Session.World.MissionManagment.Objectives;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using XnaUtils;

//namespace SolarConflict
//{
//    /// <summary>
//    /// System that generats missions, holds them and displays them to the player (switches to mission Activity)
//    /// </summary>
//    public class MissionDispatcherSystem : AgentSystem, IInteractionSystem
//    {
//        public List<IMissionGenerator> MissionList { get; private set; }

//        public string ActivityName;
//        public string ActivityParams; 
//        //  private List<IMissionFactory> _missionFactories;


//        public MissionDispatcherSystem()
//        {
//            MissionList = new List<IMissionGenerator>();
//        }

//        public string GetInteractionText(Agent agent, GameEngine gameEngine, Agent playerAgent)
//        {
//            return null;
//        }

       

        

//        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
//        {
//            return false;
//        }

//        public override AgentSystem GetWorkingCopy()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Interact(Agent agent, GameEngine gameEngine, Agent playerAgent) //Move to scene
//        {
//            if (gameEngine.Scene != null)
//            {
//                ActivityParameters activityParams = new ActivityParameters(ActivityParams);
//                activityParams.DataParams.Add("calling_agent", agent);
//                activityParams.ParamDictionary.Add("faction_index", ((int)agent.FactionType).ToString());
//                activityParams.ParamDictionary.Add("level", gameEngine.Level.ToString());
//                gameEngine.Scene.SceneComponentSelector.SwitchActivity(ActivityName, activityParams);
//                //activityParams.DataParams.Add("Scene", _scene);
//                //Also add calling agent
//            }
//            else
//            {
//                ActivityManager.Inst.SwitchActivity(ActivityName, ActivityParams);
//            }
//            return true;
//        }
//    }
//}