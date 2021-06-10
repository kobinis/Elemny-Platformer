using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode
{
    [Serializable]
    public abstract class ColorUpdater
    {

        public enum Type
        {
            FadeOut,
            FadeOutSlow,
            FadeIn,
            FadeInOut
        }

        public static ColorUpdater FadeOut = new UpdateColorFade();
        public static ColorUpdater FadeOutSlow = new UpdateColorFade(5, -5);
        public static ColorUpdater FadeIn = new UpdateColorFade(0, 1);
        public static ColorUpdater FadeInOut = new UpdateColorFadeInOut();

        private static Dictionary<string, ColorUpdater> _colorUpdateBank;
        static ColorUpdater()
        {
            _colorUpdateBank = new Dictionary<string, ColorUpdater>();
            _colorUpdateBank.Add("FadeOut", FadeOut);
            _colorUpdateBank.Add("FadeOutSlow", FadeOutSlow);
            _colorUpdateBank.Add("FadeIn", FadeIn);
            _colorUpdateBank.Add("FadeInOut", FadeInOut);
        }

        public static ColorUpdater Factory(string param)
        {
            if (_colorUpdateBank.ContainsKey(param))
                return _colorUpdateBank[param]; //fix
            else
                throw new Exception();
               // return null;
        }


        public static string GetFactoryParams(ColorUpdater colorUpdate)
        {
            foreach (var item in _colorUpdateBank)
            {
                if (item.Value == colorUpdate)
                    return item.Key;
            }

            return null;
        }


        public static ColorUpdater GetColorUpdater(Type type)
        {
            ColorUpdater result = null;

            switch (type)
            {
                case Type.FadeOut:
                    result = new UpdateColorFade();
                    break;
                case Type.FadeOutSlow:
                    result = new UpdateColorFade(5, -5);

                    break;
                case Type.FadeIn:
                    result = new UpdateColorFade(0, 1);

                    break;
                case Type.FadeInOut:
                    result = new UpdateColorFadeInOut();

                    break;
            }

            return result;
        }

        public abstract void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine);
    }
}
