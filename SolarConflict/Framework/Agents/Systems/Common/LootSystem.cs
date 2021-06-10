using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaUtils;
using SolarConflict.GameContent.Projectiles;
using SolarConflict.Framework.Agents.Systems;

namespace SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems
{
    [Serializable]
    public class LootSystem : AgentSystem //TODO: change
    {
        public IEmitter LootEmitter;
        private ParamEmitter _emitter;
        private IEmitter _cashEmitter10;
        private IEmitter _cashEmitter100;


        public LootSystem()
        {
            _cashEmitter10 = ContentBank.Inst.GetEmitter(typeof(CashDrop10).Name);
            _cashEmitter100 = ContentBank.Inst.GetEmitter(typeof(CashDrop100).Name);
            _emitter = new ParamEmitter();
            _emitter.VelocityAngleRange = 360;
            _emitter.VelocityMagMin = 2;
            _emitter.VelocityMagRange = 5;
            _emitter.VelocityAngleType = ParamEmitter.EmitterVelocityAngle.Random;
            _emitter.VelocityMagType = ParamEmitter.EmitterVelocityMag.Random;
            _emitter.RotationType = ParamEmitter.EmitterRotation.Random;
            _emitter.RotationRange = 360;
        }

        public LootSystem(string lootEmitterID):this()
        {
            LootEmitter = ContentBank.Inst.GetEmitter(lootEmitterID);
        }

        public override AgentSystem GetWorkingCopy()
        {
            return this;//(LootSystem)MemberwiseClone();
        }

        public override bool Update(Agent agent, GameEngine gameEngine, Vector2 initPosition, float initRotation, bool tryActivate = false)
        {
            //gameEngine.Scene.SetText(.ToString()) ;
            if ((agent.ControlSignal & ControlSignals.OnDestroyed) > 0)
            {
                float value = 0;
                for (int i = 0; i < agent.ItemSlotsContainer.Count; i++)
                {
                    if (agent.ItemSlotsContainer[i].Item != null)
                        value += agent.ItemSlotsContainer[i].Item.GetStackSellPrice(); //TODO: change
                }
                value = value / 10;
                int particleNum = (int)(value / 100);
                _emitter.Emitter = _cashEmitter100;
                _emitter.MinNumberOfGameObjects = particleNum;
                _emitter.Emit(gameEngine, agent, FactionType.Neutral, initPosition, agent.Velocity, agent.Rotation);

                value -= particleNum * 100;
                _emitter.Emitter = _cashEmitter10;
                _emitter.MinNumberOfGameObjects = (int)(value / 10);
                _emitter.Emit(gameEngine, agent, FactionType.Neutral, initPosition, agent.Velocity, agent.Rotation);
                LootEmitter?.Emit(gameEngine, agent, FactionType.Neutral, initPosition, agent.Velocity, agent.Rotation);

                return true;
            }
            return false;
        }
    }
}

