using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects;
using XnaUtils;

namespace SolarConflict.Framework.Agents.Systems.EmitterCallers
{
    [Serializable]
    class BeamSystem : AgentSystem
    {
        public string EmitterID { set { beamEmitter = ContentBank.Inst.GetEmitter(value); } }
        public IEmitter beamEmitter;
        public string EffectEmitterID { set { effectEmitter = ContentBank.Inst.GetEmitter(value); } }
        private IEmitter effectEmitter;
        private DummyObject _originObject;
      //  private bool _isBeamActive;
        private GameObject _beamObject;

        public BeamSystem( )
        {
            _originObject = new DummyObject();            
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
           

            if(tryActivate)
            {
                _originObject.Position = initPosition;
                _originObject.Rotation = initRotation;
                _originObject.Parent = agent;
                _originObject.Faction = agent.FactionType;

                _beamObject = beamEmitter.Emit(gameEngine, _originObject, agent.GetFactionType(), initPosition, Vector2.Zero, initRotation);
                _beamObject.Rotation = initRotation;
                if (effectEmitter != null)
                {
                    effectEmitter.Emit(gameEngine, _originObject, agent.GetFactionType(), initPosition, agent.Velocity * 0.4f + FMath.ToCartesian(7, initRotation), initRotation);
                }
                //if (!_isBeamActive)
                //{

                //}


            }
            //else
            //{
            //    if(_beamObject != null)
            //    {
            //        _beamObject.IsActive = false;
            //        _beamObject = null;
            //    }
            //}
      
            return false;
        }

        public override AgentSystem GetWorkingCopy()
        {
            BeamSystem clone = MemberwiseClone() as BeamSystem;
            clone._originObject = new DummyObject();
            clone._beamObject = null;
            return clone;
        }
    }
}
