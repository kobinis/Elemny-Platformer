//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using XnaUtils;
//using SolarConflict.Framework.Agents.Systems;

//namespace SolarConflict
//{
//    [Serializable]
//    public class ReviveSystem : AgentSystem
//    {
//        ActivationCheck activationCheck;
//        public bool RespawnInPlace;

//        public ReviveSystem()
//        {
//            activationCheck = new ActivationCheck(ControlSignals.OnDestroyed);
//            RespawnInPlace = true;
//        }

//        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
//        {
//            bool wasActive = false;
//            if (agent.ItemSlotsContainer != null && activationCheck.Check(agent, tryActivate))
//            {
//                wasActive = true;

//                Agent agentClone = agent;//.GetWorkingCopy(); //(Agent)ContentBank.Inst.GetGameObjectFactory(agent.Id).MakeGameObject(null, agent.faction, 0, null, null); //change to agent.clone
//                agentClone.SetMeterValue(MeterType.Hitpoints, agent.GetMeter(MeterType.Hitpoints).MaxValue);

//                if (RespawnInPlace)
//                {
//                    agentClone.Position = agent.Position;
//                }
//                else
//                {
//                    agentClone.Position = gameEngine.GetFaction(agent.FactionType).GetStartingPoint();
//                }
                                                                                
//                agentClone.SetMeterValue(MeterType.StunTime, 90); //TODO: make a parametre                
//                agentClone.IsActive = true;
                
//            }


//            return wasActive;
//        }
        
//        public override AgentSystem GetWorkingCopy()
//        {
//            ReviveSystem system = (ReviveSystem)MemberwiseClone();
//            system.activationCheck = activationCheck.GetWorkingCopy();
//            return system;
//        }

       


//    }
//}
