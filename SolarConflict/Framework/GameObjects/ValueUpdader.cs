using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Framework.GameObjects
{
    /// <summary>
    /// Used to update a value according to the state of a gameobject
    /// </summary>
    [Serializable]
    public class ValueUpdader
    {
        public float bias;
        public float lifetime;
        public float size;
        public float maxValue;
        public float minValue;


        public ValueUpdader(float initValue)
        {
            bias = initValue;
            lifetime = -initValue;
            maxValue = float.MaxValue;
            minValue = float.MinValue;
        }

        public ValueUpdader(float bias, float lifetime, float size)
        {
            this.bias = bias;
            this.lifetime = lifetime;
            this.size = size;
            maxValue = float.MaxValue;
            minValue = float.MinValue;
        }

        public float GetValue(GameEngine gameEngine, GameObject gameObject)
        {
            float normalizedLifetime = gameObject == null ? 0 : gameObject.NormalizedLifetime();
            float sizeValue = gameObject == null ? 0 : gameObject.Size;
            return MathHelper.Clamp(bias + normalizedLifetime * lifetime + sizeValue * size, minValue, maxValue);
        }
    }
}
