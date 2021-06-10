using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public class InitFloatConst:BaseInitFloat
    {
        public float value;

        public InitFloatConst()
        {
        }

        public InitFloatConst(float value)
        {
            this.value = value;
        }

        public override float Init(Projectile projectile, GameEngine gameEngine)
        {
            return value;
        }

        public override string GetParams()
        {
            return this.GetType().Name + "," + value.ToString();
        }

        public override BaseInitFloat LoadFromParams(string param)
        {
            string[] paramStrings = param.Split(',');
            float value = float.Parse(paramStrings[paramStrings.Length - 1],new CultureInfo("en-US"));
            return new InitFloatConst(value);
        }

    }
}
