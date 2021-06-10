using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using SolarConflict.NewContent.Projectiles;
using SolarConflict.GameContent.Emitters;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc {
    [Serializable]
    class RewindSystem : AgentSystem {

        IEmitter _warpEffect;
        IEmitter _activationEffect;
            

        int _cooldown;
        int _maxCooldown;

        MeterType[] _metersToRewind;
        float[] _meterValuesOnRewind;
        Vector2 _positionOnRewind;

        /// <summary>If non-negative, the rewind effect is active, and will trigger at this game time</summary>
        int _triggerTime;

        /// <summary>Time between activating this system and the timewarp triggering</summary> // TODO: redoc
        int _rewindDuration;

        public RewindSystem(IEnumerable<MeterType> metersToRewind, int rewindDuration, int maxCooldown = 0) {
            _maxCooldown = maxCooldown;
            _metersToRewind = metersToRewind.ToArray();
            _rewindDuration = rewindDuration;
            _triggerTime = -1;
            _activationEffect = ContentBank.Inst.GetEmitter(typeof(HyperSpaceJumpFx).Name);
            _warpEffect = ContentBank.Inst.GetEmitter(typeof(HyperSpaceJumpFx).Name);
        }

        public override AgentSystem GetWorkingCopy() {
            return new RewindSystem(_metersToRewind, _rewindDuration, _maxCooldown);
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false) {

            if (_triggerTime < 0f) {
                // System inactive, we trying to activate?
                if (_cooldown <= 0 && tryActivate) {
                    // Start rewind effect
                    _activationEffect?.Emit(gameEngine, agent, agent.FactionType, agent.Position, Vector2.Zero, agent.Rotation, maxLifetime: _rewindDuration, size:agent.Size);
                    _meterValuesOnRewind = _metersToRewind.Select(m => agent.GetMeterValue(m)).ToArray();
                    _positionOnRewind = agent.Position;

                    _triggerTime = gameEngine.FrameCounter + _rewindDuration;

                    return true;
                }
            } else {
                // System active, are we rewinding yet?
                if (_triggerTime <= gameEngine.FrameCounter) {
                    for (int i = 0; i < _metersToRewind.Length; ++i) {
                        agent.SetMeterValue(_metersToRewind[i], _meterValuesOnRewind[i]);
                    }
                    agent.Position = _positionOnRewind;                    
                    _triggerTime = -1;
                    _cooldown = _maxCooldown;
                    _warpEffect?.Emit(gameEngine, agent, agent.FactionType, agent.Position, agent.Velocity, agent.Rotation);
                    return true;
                }                                
            }

            // TODO: effect emitters (indicator/spectral image at rewind position, flash or something on rewind)

            return false;
        }        
    }
}
