using Microsoft.Xna.Framework;
using SolarConflict.Framework.Agents.Systems;
using System;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    public class TimeWarp : AgentSystem
    {        
        public ActivationCheck ActivationCheck;     
        public int ActiveTime;        
        public int CooldownTime;                

        private int cooldown;
        private int active;
        int currentTime = -1;
                        
        public TimeWarp(ControlSignals signal, int cooldownTime)
        {
            ActiveTime = 60*10;
            ActivationCheck = new ActivationCheck(signal);        
            cooldown = 0;            
            active = 0;
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate)
        {
            bool wasActivated = false;
            if (cooldown <= 0 && (ActivationCheck == null || ActivationCheck.Check(agent, tryActivate)))
            {
                cooldown = CooldownTime;
                active = ActiveTime;                                
                if (ActivationCheck != null)
                   ActivationCheck.DrainCost(agent);
            }

            if (active > 0 && gameEngine.FrameCounter != currentTime)
            {
                currentTime = gameEngine.FrameCounter;
                agent.Update(gameEngine);
            }

            active--;
            cooldown = Math.Max(cooldown - 1, 0); //maybe remove the max            
            return wasActivated;
        }        

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone();
        }

        public override float GetCooldownTime()
        {
            return CooldownTime;
        }

        public override float GetCooldown()
        {
            return cooldown;
        }

    }
}
