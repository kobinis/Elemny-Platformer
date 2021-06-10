using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.IO;
using SolarConflict.Framework;
using XnaUtils;

namespace SolarConflict
{
    //TODO: split to two classes 
    [Serializable]
    public class GroupEmitter : IEmitter
    {
        public enum EmitterType { 
            /// <summary>
            /// Calls all emitters according to their probability
            /// </summary>
            All,
            /// <summary>
            /// Calls a random emitter according to its relative probability
            /// </summary>
            RandomOne 
        }

        public string ID { get; set; }
        public EmitterType EmitType { get; set; }
        public float RandomVelocityRange { get; set; }
        public float RandomVelocityBase { get; set; }
        public float RotationSpeed { get; set; }  
        public float RefVelocityMult { get; set; }

        private List<IEmitter> _emitterList;
        private List<float> _probabilityList;
        private bool _isProbDifferentFromOne;
        private float _probListSum;
                                              
        public GroupEmitter()
        {            
            _emitterList = new List<IEmitter>();
            _probabilityList = new List<float>();
            RefVelocityMult = 1;                 
        }

        public void MakeRandomVelocitiy()
        {
            RandomVelocityBase = 1;
            RandomVelocityRange = 10;
            RotationSpeed = 0.01f;
        }

        public void AddEmitter(IEmitter emitter, float probability = 1) 
        {
            if (emitter == null)
                return;
            if (probability != 1)
            {
                _isProbDifferentFromOne = true;
            }
            _probListSum += probability;
            _probabilityList.Add(probability);
            _emitterList.Add(emitter);
        }
        
        public void AddEmitter(string emitterId, float probability = 1)
        {
            AddEmitter(ContentBank.Inst.GetEmitter(emitterId), probability);            
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            GameObject gameObject = null;
            Vector2 randomVelocity;
            switch (EmitType)
            {
                case EmitterType.All:
                    if (_isProbDifferentFromOne)
                    {
                        for (int i = 0; i < _emitterList.Count; i++)
                        {
                            if (gameEngine.Rand.NextDouble() < _probabilityList[i])
                            {
                                randomVelocity = CalculateRandomVelocity(gameEngine.Rand);
                                gameObject = _emitterList[i].Emit(gameEngine, parent, faction, refPosition, refVelocity * RefVelocityMult + randomVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
                            }
                        }
                    }
                    else
                    {
                        foreach (IEmitter emitter in _emitterList)
                        {
                            randomVelocity = CalculateRandomVelocity(gameEngine.Rand);
                            gameObject = emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity * RefVelocityMult + randomVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
                        }
                    }
                    break;

                case EmitterType.RandomOne: //TODO: don't work!!!didn't check it
                    randomVelocity = CalculateRandomVelocity(gameEngine.Rand);
                    if (_isProbDifferentFromOne)
                    {
                        float selectedRange = (float)gameEngine.Rand.NextDouble() * _probListSum;
                        float sum = 0;
                        int index = 0;
                        while(sum < selectedRange)
                        {
                            sum += _probabilityList[index];
                            index++;
                        }
                        index = Math.Max(0, index - 1);                       
                        gameObject = _emitterList[index].Emit(gameEngine, parent, faction, refPosition, refVelocity * RefVelocityMult + randomVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);                        
                    }
                    else
                    {                       
                        int index = gameEngine.Rand.Next(_emitterList.Count);
                        gameObject = _emitterList[index].Emit(gameEngine, parent, faction, refPosition, refVelocity * RefVelocityMult + randomVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);                        
                    }
                    break;                                
                default: //TODO: maybe throw exception, write to log                    
                    break;
            }

            return gameObject;
        }

        private Vector2 CalculateRandomVelocity(Random random)
        {
            if(RandomVelocityBase > 0)
            {
                float speed = RandomVelocityBase;
                float angle = (float)random.NextDouble() * MathHelper.TwoPi;
                Vector2 velocity = new Vector2((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed);
                return velocity;
            }
            return Vector2.Zero;
        }
             
    }
}
