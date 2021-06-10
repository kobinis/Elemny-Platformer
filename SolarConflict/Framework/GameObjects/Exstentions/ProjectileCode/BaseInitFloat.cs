using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode
{
    [Serializable]
    public abstract class BaseInitFloat
    {
        public abstract float Init(Projectile projectile, GameEngine gameEngine);  //TODO: maybe add game engine???

        public virtual string GetParams()
        {
            return this.GetType().Name;
        }

        public virtual BaseInitFloat LoadFromParams(string param)
        {
            return this;
        }

        static Dictionary<string, BaseInitFloat> initFloatBank;

        static BaseInitFloat()
        {
            initFloatBank = new Dictionary<string, BaseInitFloat>();
            BaseInitFloat initFloat;
            initFloat = new InitFloatConst(); //TODO: do it with reflection?
            initFloatBank.Add(initFloat.GetType().Name, initFloat);
            initFloat = new InitFloatParentSize();
            initFloatBank.Add(initFloat.GetType().Name, initFloat);
            initFloat = new InitFloatParentDamageTaken();
            initFloatBank.Add(initFloat.GetType().Name, initFloat);
            initFloat = new InitFloatRandom();
            initFloatBank.Add(initFloat.GetType().Name, initFloat);
        }

        public static BaseInitFloat Factory(string param)
        {
            float value;
            if (float.TryParse(param, NumberStyles.Any, new CultureInfo("en-US"), out value))
            {
                return new InitFloatConst(value);
            }
            else
            {
                string[] paramArray = param.Split(',');
                if (initFloatBank[paramArray[0]].GetType().Name == paramArray[0])
                {
                    return initFloatBank[paramArray[0]].LoadFromParams(param);
                }
                else
                {
                    return initFloatBank[paramArray[0]];
                }

            }
        }

        public static string GetInitFloatParam(BaseInitFloat baseinitFloat)
        {
            if (baseinitFloat != null)
            {
                if (baseinitFloat.GetType().Name == typeof(InitFloatConst).Name)
                {
                    string[] constParams = baseinitFloat.GetParams().Split(',');
                    return constParams[1];
                }
                else
                {
                    //ToDo: add saving const
                    return baseinitFloat.GetParams();
                }
            }
            else
            {
                return null;
            }
        }

    }
}
