using Microsoft.Xna.Framework;
using SolarConflict.Framework.MetaGame.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.Agents.Systems {
    
    /// <summary>Memorizes a map node, warps the player's fleet to it</summary>
    [Serializable]
    class EmergencyWarpSystem : AgentSystem 
    {
        private const int CooldownTime = 60;
        private const float HitpointActivationValue = 0.5f;

        public IEmitter Effect;
        private int cooldown;
        
        public EmergencyWarpSystem() {
            cooldown = 0;
            Effect = ContentBank.Inst.GetEmitter(Consts.WARPIN_EFFECT);
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate) {
            // If anchor isn't set, have it default to current system
            int targetNode = 0;// gameEngine.GetFaction(agent.GetFactionType()).HomeWorldIndex;
            if (!agent.IsActive || agent.GetFactionType() != FactionType.Player) // No warping out the fleet if we're destroyed in one shot
                return false;
            cooldown--;// gameEngine.GetFaction(agent.GetFactionType()).WarpCooldown;
            if (cooldown <= 0 && agent.GetMeter(MeterType.Hitpoints).NormalizedValue < HitpointActivationValue && targetNode >= 0&& targetNode != GalaxyMap.Inst.CurrentNodeIndex && gameEngine.FrameCounter > 10) {
                // Warp out
                cooldown = CooldownTime;
                //gameEngine.GetFaction(agent.GetFactionType()).WarpCooldown = Faction.WARP_COOLDOWNTIME;
                agent.AddMeterValue(MeterType.Hitpoints, 4000);
                Effect.Emit(gameEngine, agent, agent.GetFactionType(), agent.Position, agent.Velocity, agent.Rotation);
               // gameEngine.Scene.fadeAlpha += 0.8f;// 0.01f;
                gameEngine.Scene.WarpAtEndOfUpdate(targetNode);
                return true;
            }    
            
            return false;
        }

        public override AgentSystem GetWorkingCopy() {
            return (AgentSystem)MemberwiseClone();
        }

        public override float GetCooldown()
        {
            return cooldown;
        }

    }
}
