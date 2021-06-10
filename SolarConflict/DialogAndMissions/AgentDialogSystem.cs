using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using SolarConflict.Framework.Agents;
using SolarConflict.Framework.Agents.Systems;
using SolarConflict.Framework.Scenes.DialogEngine;
using SolarConflict.Framework.Utils;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    public class AgentDialogSystem : IInteractionSystem
    {                
        public Dialog diloagNode;
        public string InteractionText;

        public AgentDialogSystem(string textAssetID)
        {
            diloagNode = TextBank.Inst.GetDialogNode(textAssetID);
            InteractionText = "Interact";
        }

        public void AddText(Dialog dialogSequence)
        {

        }
       
       

        public string GetInteractionText(Agent agent, GameEngine gameEngine, Agent playerAgent)
        {
            return InteractionText;
        }

        public bool Interact(Agent agent, GameEngine gameEngine, Agent playerAgent)
        {
            //Set Charechter
            gameEngine.Scene.DialogAgent = agent;
            gameEngine.Scene.DialogManager.AddDialog(diloagNode);
            diloagNode.IsFinished = false;
            //in range, allie
            return false;
        }

        //public override AgentSystem GetWorkingCopy()
        //{
        //    AgentDialogSystem system = (AgentDialogSystem)MemberwiseClone();
        //    return system;
        //}
    }
}


