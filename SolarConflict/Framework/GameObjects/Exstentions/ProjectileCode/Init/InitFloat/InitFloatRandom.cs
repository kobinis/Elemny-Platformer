using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    public class InitFloatRandom : BaseInitFloat
    {
        public float MinValue;
        public float Range;

        public InitFloatRandom() { }

        public InitFloatRandom(float minValue, float range)
        {
            MinValue = minValue;
            Range = range;
        }

        public override float Init(Projectile projectile, GameEngine gameEngine)
        {
            return ((float)gameEngine.Rand.NextDouble()) * Range + MinValue;
        }

        public override string GetParams()
        {
            return this.GetType().Name + "," + MinValue.ToString() + "," + Range.ToString();
        }

        public float GetAvarageLifetime()
        {
            return MinValue + Range / 2f;
        }

        public override BaseInitFloat LoadFromParams(string param)
        {
            string[] stringParams = param.Split(',');
            float minValue = float.Parse(stringParams[1], new CultureInfo("en-US"));
            float range = float.Parse(stringParams[2], new CultureInfo("en-US"));
            return new InitFloatRandom(minValue, range);            
        }
    }
}
