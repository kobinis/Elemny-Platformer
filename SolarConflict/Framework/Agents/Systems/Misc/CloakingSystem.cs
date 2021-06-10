using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc
{
    [Serializable]
    public class CloakingSystem : AgentSystem
    {                                       
        public int Cooldown { get; set; }     
        public string ActivationEmitterID
        {            
            set { _activationEmitter = ContentBank.Inst.GetEmitter(value); }
        }        
        //public float CloackingDepth { get; set; }

        private IEmitter _activationEmitter;
        private int _currentCooldown;
        private bool _isCloakedActivated;
        /// <summary>Amount of time for which the system'll be disrupted if its agent takes damage</summary>
        /// <remarks>Note that this is distinct from putting the system on cooldown (so a cloak could have a limited duration and a lengthy cooldown, neither of which would affect or be affected
        /// by flickering)</remarks>
        private int _flickerDurationTimer;


        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            bool wasActivated = false;
            if(!agent.IsCloaked)
            {                
                if (_isCloakedActivated) 
                {
                    // Looks like cloak was broken this frame (or the last, after the last Update() call)
                    _currentCooldown = Cooldown;
                    _isCloakedActivated = false;
                }

                if (_currentCooldown<=0)
                {
                    // Reactivate cloak
                    _isCloakedActivated = true;
                    if (_activationEmitter != null)
                        _activationEmitter.Emit(gameEngine, agent, agent.FactionType, initPosition, agent.Velocity, initRotation);
                    //TODO: maybe delay the actual cloaking after the effect ends
                    agent.SetMeterValue(MeterType.Cloak, 100); //TODO: think about the value (1)
                    wasActivated = true;
                }
                _currentCooldown--;
            }
            else
            {
                
            }
            return wasActivated;
        }

        public override float GetCooldown()
        {
            return _currentCooldown;  
        }

        public override AgentSystem GetWorkingCopy()
        {
            return (AgentSystem)MemberwiseClone();
        }

        
    }
}
