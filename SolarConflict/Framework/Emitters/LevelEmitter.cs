using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Emitters
{
    /// <summary>
    /// Calls emitters from a list according to the level of the parent or the node
    /// Emitter[0] is the default emitter and if it's different then null it also will be called
    /// </summary>
    [Serializable]
    class LevelEmitter : IEmitter
    {
        public string ID { get; set; }
        IEmitter[] emitters;
        public LevelEmitter(int maxLevel = 10)
        {
            emitters = new IEmitter[maxLevel + 1];
        }

        public LevelEmitter(IEmitter[] emittersArray)
        {
            int size = 0;
            for (int i = 0; i < emittersArray.Length; i++)
            {
                if (emittersArray[i] != null)
                    size = i + 1;
            }
            emitters = new IEmitter[size];
            for (int i = 0; i < size; i++)
            {
                emitters[i] = emittersArray[i];
            }
        }


        public GameObject Emit(GameEngine gameEngine, GameObject parent, FactionType faction, Vector2 refPosition, Vector2 refVelocity, float refRotation, float refRotationSpeed = 0, int maxLifetime = 0, float? size = default(float?), Color? color = default(Color?), float param = 0)
        {
            emitters[0]?.Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);

            int level = gameEngine.Level;
            if (parent != null)
                level = parent.Level;
            if (param > 0)
                level = (int)param;
            
            level = Math.Min(Math.Max(level, 1), emitters.Length - 1);
            var emitter = emitters[level];
            if(emitter != null)
            {
                return emitter.Emit(gameEngine, parent, faction, refPosition, refVelocity, refRotation, refRotationSpeed, maxLifetime, size, color, param);
            }
            return null;
        }
    }
}
