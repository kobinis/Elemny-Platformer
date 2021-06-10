using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public class UpdateHitPoints : BaseUpdate
    {
        public float MaxHP { get; set; }
        public float MinHP { get; set; }
        public float GenerationRatePerFrame { get; set; }
        public float GenerationRatePerSecond
        {
            get  { return GenerationRatePerFrame * 60; }
            set  { GenerationRatePerFrame = value / 60; }
        }

        public UpdateHitPoints()
            :this (0f, 0f, float.MaxValue)
        {
        }

        public UpdateHitPoints(float generationRate, float minHP, float maxHP)
        {
            GenerationRatePerFrame = generationRate;
            MaxHP = maxHP;
            MinHP = minHP;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            projectile.hitPoints = MathHelper.Clamp(projectile.hitPoints + GenerationRatePerFrame, MinHP, MaxHP);
        }
    }
}
