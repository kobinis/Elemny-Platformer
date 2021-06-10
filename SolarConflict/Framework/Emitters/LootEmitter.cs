using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SolarConflict.Framework;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    public class LootEmitter : IEmitter
    {        
        [Serializable]
        private struct LootEntry
        {
            public LootEntry(IEmitter emitter, float probability, int amountMin, int amountRange)
            {
                Emitter = emitter;
                Probability = probability;
                AmountMin = amountMin;
                AmountRange = amountRange;
            }

            public IEmitter Emitter;
            public float Probability;
            public int AmountMin;
            public int AmountRange;
        }

        public string ID { get; set; }
        public float RandomVelocityBase;
        public float RandomVelocityRange;
        private List<LootEntry> _entryList;        
        private bool _isProbDifferentFromOne;
        private float _probListSum;


        public LootEmitter()
        {
            _entryList = new List<LootEntry>();
            RandomVelocityBase = 5;
            RandomVelocityRange = 10;
        }

        public void AddEmitter(IEmitter emitter, float probability = 1, int amountMin = 0, int amountRange = 0)
        {
            if (probability != 1)
            {
                _isProbDifferentFromOne = true;
            }
            _probListSum += probability;            
            _entryList.Add(new LootEntry(emitter, probability, amountMin, amountRange));
        }

        public void AddEmitter(string emitterId, float probability = 1, int amountMin = 0, int amountRange = 0)
        {
            AddEmitter(ContentBank.Inst.GetEmitter(emitterId), probability, amountMin, amountRange);
        }

        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0,
            int maxLifetime = 0, float? size = null, Color? color = null, float param = 0)
        {
            GameObject gameObject = null;
            Vector2 randomVelocity;
            float? stack = null;
            if (_isProbDifferentFromOne)
            {
                for (int i = 0; i < _entryList.Count; i++)
                {
                    LootEntry entry = _entryList[i];
                    if (gameEngine.Rand.NextDouble() < entry.Probability)
                    {          
                        if(entry.AmountMin > 0)
                        {
                            if (entry.AmountRange > 0)
                            {
                                stack = entry.AmountMin + gameEngine.Rand.Next(entry.AmountRange);
                            }
                            else
                            {
                                stack = entry.AmountMin;
                            }                            
                        }
                        float rotation = (float)gameEngine.Rand.NextDouble()* MathHelper.TwoPi;
                        randomVelocity = CalculateRandomVelocity(gameEngine.Rand);
                        gameObject = entry.Emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity  + randomVelocity, rotation, refRotationSpeed, maxLifetime, stack, color, param);                        
                    }
                }
            }
            else
            {
                foreach (LootEntry entry in _entryList)
                {
                    if (entry.AmountMin > 0)
                    {
                        if (entry.AmountRange > 0)
                        {
                            stack = entry.AmountMin + gameEngine.Rand.Next(entry.AmountRange);
                        }
                        else
                        {
                            stack = entry.AmountMin;
                        }
                    }
                    float rotation = (float)gameEngine.Rand.NextDouble() * MathHelper.TwoPi;
                    randomVelocity = CalculateRandomVelocity(gameEngine.Rand);
                    gameObject = entry.Emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity + randomVelocity, rotation, refRotationSpeed, maxLifetime, stack, color);
                }
            }                                    
            return gameObject;
        }

        private Vector2 CalculateRandomVelocity(Random random)
        {
            if (RandomVelocityBase > 0)
            {
                float speed = RandomVelocityBase + random.NextFloat() * RandomVelocityRange;
                float angle = (float)random.NextDouble() * MathHelper.TwoPi;
                Vector2 velocity = new Vector2((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed);
                return velocity;
            }
            return Vector2.Zero;
        }

        //To be used 
        public IEmitter GetMainEmitter()
        {
            return _entryList[0].Emitter;
        }

    }
}
