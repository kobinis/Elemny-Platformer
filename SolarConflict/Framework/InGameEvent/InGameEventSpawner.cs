using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.InGameEvent
{
    [Serializable]
    public abstract class InGameEventSpawner : GameProcess
    {
        public string ID { get; set; }        
        public IEventActivationCheck ActivationCheck;
        public int ActivationCooldownTime = 0;
        public float ActivationProbability = 1;
        protected int _timeSinceActivation = 0;    
        public bool IsOneTime;    

        public override void Update(GameEngine gameEngine)
        {            
            _timeSinceActivation++;
            if (_timeSinceActivation >= ActivationCooldownTime && gameEngine.FrameCounter % 60 == 0 && (ActivationProbability == 1 || gameEngine.Rand.NextDouble() < ActivationProbability) &&
                (ActivationCheck == null || ActivationCheck.CheckActivation(gameEngine.PlayerAgent, gameEngine)))
            {
                GameProcess gameEvent = MakeEvent(gameEngine);
                if(IsOneTime)
                {
                    Finished = true;
                }
                _timeSinceActivation = 0;
                if (gameEvent != null)
                {
                    gameEngine.AddGameProcces(gameEvent);
                }
            }            
        }

        public abstract GameProcess MakeEvent(GameEngine gameEngine);



    }
}
