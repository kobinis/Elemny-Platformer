using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XnaUtils;

namespace SolarConflict {
    [Serializable]
    class EmergencySalvagerAI : IAgentControl {
        public int ID { get; set; }
        
        
        public ControlSignals Update(Agent agent, GameEngine gameEngine, ref Vector2[] analogDirections) {
            // Seek mothership
            var target = gameEngine.GetFaction(agent.FactionType).Mothership;
            
            if (target?.IsActive != true)           
                // Nowhere to return      
                return ControlSignals.Brake;

            var result = ControlSignals.Up;
            
            agent.SetTarget(target, TargetType.Goal);            

            // Rotate to face target            
            analogDirections[0] = (target.Position - agent.Position).Normalized();            

            if (GameObject.DistanceFromEdge(agent, agent.GetTarget(gameEngine, TargetType.Goal)) < 500) {
                // Target reached
                result &= ~ControlSignals.Up;
                result |= ControlSignals.Brake;
            }

            return result;                 
        }

        public ControlAI GetWorkingCopy() {
            return (ControlAI)MemberwiseClone();
        }

        public int GetCameraPriority() {
            return 1;
        }

        IAgentControl IAgentControl.GetWorkingCopy() {
            return MemberwiseClone() as IAgentControl;
        }
    }
}
