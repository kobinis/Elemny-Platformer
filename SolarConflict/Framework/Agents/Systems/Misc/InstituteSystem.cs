//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using XnaUtils;
//using SolarConflict.GameContent;
//using SolarConflict.Framework;
//using SolarConflict.Framework.Agents.Systems;
//using SolarConflict.Framework.Agents;

//namespace SolarConflict
//{
//    [Serializable]
//    public class InstituteSystem : AgentSystem, IInteractionSystem //TODO: Create a system that on Use(F) activates a system
//    {
//        public string ActivityName;
//        public string ActivityParams;
//        public Activity TargetActivity;
//        public bool Persistent;

//        private string _messageID;

//        public string ID { get; set; }
//        public string SellerImageID { get; set; }
//        public string Text { get; set; }

//        public float Range = 600;

//        public InstituteSystem()
//        {
//            Text = "F";
//        }

//        public override AgentSystem GetWorkingCopy()
//        {
//            return (AgentSystem)MemberwiseClone();
//        }


//        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
//        {
//            bool wasActivated = false;
//            GameObject target = null;
//            if (gameEngine.Scene != null)
//            {
//                target = gameEngine.Scene.FindPlayer();
//            }
//            if ((target != null && target.GetControlType() == AgentControlType.Player) && (GameObject.DistanceFromEdge(target, agent) < Range + target.Size))
//            {
//                //parse massage
//                // _messageID = gameEngine.Scene.DialogManagerOld.AddDialogBox(Text, SellerImageID, boxID: _messageID, maxLifetime: 10, isFixedSize:false);
//                if (gameEngine.Scene.PlayersManager.players[0].IsCommandClicked(PlayerCommand.Use)) //Fix Docking
//                {
//                    // target = this.GetTarget(gameEngine);
//                    if (target.GetControlType() == AgentControlType.Player && ActivityName != null)
//                    {
//                        if (gameEngine.Scene != null)
//                        {
//                            gameEngine.Scene.SceneComponentSelector.SwitchActivity(ActivityName, ActivityParams);
//                            //activityParams.DataParams.Add("Scene", _scene);
//                        }
//                        else
//                        {
//                            ActivityManager.Inst.SwitchActivity(ActivityName, ActivityParams);
//                        }
//                        wasActivated = true;
//                    }
//                }
//            }
//            return wasActivated;
//        }

//        public string GetInteractionText(Agent agent, GameEngine gameEngine, Agent playerAgent)
//        {
//            throw new NotImplementedException();
//        }

//        public bool Interact(Agent agent, GameEngine gameEngine, Agent playerAgent)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}



