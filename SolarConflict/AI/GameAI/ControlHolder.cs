using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.AI;
using XnaUtils;
using SolarConflict.Framework.PlayersManagement;

namespace SolarConflict
{
    public enum AgentControlType { AI, Player, CoPlayerOrAI1, CoPlayerOrAI2, None }
        
    [Serializable]
    public class ControlHolder //No need to be in a class, can move out
    {        
        public AgentControlType ControlType { get; set; }
        public IAgentControl controlAi;

        public ControlHolder()
        {
           // _sensorValues = new Dictionary<SensorType, AiFloat>();     
            ControlType = AgentControlType.AI;
            controlAi = null;      
        }

        public void SetAIControl(IAgentControl control)
        {
            controlAi = control;
        }

        public void SetAIControl(int id)
        {
            controlAi = AIBank.Inst.GetControl(id);
        }


        public ControlSignals Update(Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections)
        {
            if (agent.Lifetime == 0) //TODO: move to init gameobject(Agent)
                analogDirections[0] = FMath.ToCartesian(1, agent.Rotation);
            //_sensorValues.Clear();
            switch (ControlType)
            {
                case AgentControlType.AI:
                    if (controlAi != null)
                    {
                        ControlSignals signals = controlAi.Update(agent, gameEngine, ref analogDirections);                                              
                        return signals;
                    }
                    return 0;                    
                case AgentControlType.Player:                                        
                    if (gameEngine.Scene != null) //TODO: refactor?
                    {
                        return gameEngine.Scene.PlayersManager.players[0].UpdateAgent(0, agent, gameEngine, ref analogDirections);                        
                    } //maybe else run AI
                    return 0;                    
                case AgentControlType.CoPlayerOrAI1: //depentend on the number of the players,
                    if (gameEngine.Scene != null && gameEngine.Scene.PlayersManager.players[1] != null && PlayersManager.IsPlayerActive(gameEngine.Scene, 1))
                    {
                        return gameEngine.Scene.PlayersManager.players[1].UpdateAgent(1,agent, gameEngine, ref analogDirections);
                    }
                    else
                    {
                        if (controlAi != null)
                            return controlAi.Update(agent, gameEngine, ref analogDirections);
                    }
                    return 0;
                default:
                    break;
            }
           
            

            return 0;
        }
        
        public ControlHolder GetWorkingCopy() //TODO:Change
        {
            ControlHolder clone = (ControlHolder)MemberwiseClone();
            if(controlAi != null)
                clone.controlAi = clone.controlAi.GetWorkingCopy();
            //_sensorValues = new Dictionary<SensorType, AiFloat>();
            return clone;
        }
    }
}
