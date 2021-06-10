using Microsoft.Xna.Framework;
using SolarConflict.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Framework.Graphics;

namespace SolarConflict.Generation
{
    class ScalingUtils
    {
        public const int NumLevels = 10;
        //Base values for level 0
        private const float BaseDamagePerSec = 100;
        private const float BaseShieldCapacity = BaseDamagePerSec * 5f;
        private const float BaseShieldGenerationPerSec = BaseShieldCapacity / 20f;

        private const float BaseEnergyCostPerSec = 60;
        private const float BaseEnergyCapacity = BaseEnergyCostPerSec * 30;
        private const float BaseEnergyGenerationPerSec = BaseEnergyCostPerSec * 1.5f;

        private const int BaseRange = 1500;

        private const int BaseCost = 200;

        /// <summary>Scales a base item cost with item tier</summary>
        public static int ScaleCost(int level, int baseCost = BaseCost)
        {
            return (int)(baseCost * Math.Pow(1.5,(level + 1)));
        }


        public static float ScaleDamagePerFrame(int level, float baseValue = BaseDamagePerSec)
        {
            return baseValue * (float)Math.Pow (level + 1,1.3) / 60f / 2;
        }

        public static float ScaleEnergyCostPerFrame(int level, float baseValue = BaseEnergyCostPerSec)
        {
            return baseValue * (level + 1) /60;
        }

        public static float ScaleEnergyCapacity(int level, float baseValue = BaseEnergyCapacity)
        {
            return baseValue * (level + 1);
        }

        public static float ScaleEnergyGenerationPerFrame(int level, float baseValue = BaseEnergyGenerationPerSec)
        {
            return baseValue * (level + 1) / 60f;
        }

        public static float ScaleShieldCapacity(int level, float baseValue = BaseShieldCapacity)
        {
            return baseValue * (level) * 1.5f;
        }

        public static float ScaleShieldGenerationPerFrame(int level, float baseValue = BaseShieldGenerationPerSec)
        {
            return baseValue * (level + 1)/ 60f;
        }

        public static float ScaleShotRange(int level, float delta = 500)
        {
            return Math.Min( BaseRange + delta*level, 5000);
        }

        public static float ScaleEngineSpeed(int level) {
            //return baseValue * (level + 1);
            return  Math.Max(Math.Min(5 + 7f * (level + 1), 80), 20);
        }

        public static Color EffectColor(int level)
        {
            return GraphicsUtils.HsvToRgb(level / (float)NumLevels * 300, 1, 1);
        }

        public static string NameFromLevel(string baseName, int level)
        {
            if(level == 0)
            {
                return "Basic " + baseName;
            }
            else
            {
                return baseName + " Mk" + level.ToString();
            }
        }
    }
}
