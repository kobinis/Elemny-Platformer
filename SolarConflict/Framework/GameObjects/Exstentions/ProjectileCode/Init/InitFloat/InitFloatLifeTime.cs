using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public class InitFloatParentLifeTime : BaseInitFloat
    {
        public float SizeMult;
        public float SizeOffset;

        public InitFloatParentLifeTime()
        {
            SizeMult = 1;
            SizeOffset = 0;
        }

        public InitFloatParentLifeTime(float sizeMult, float sizeAdd)
        {
            this.SizeMult = sizeMult;
            this.SizeOffset = sizeAdd;
        }

        public override float Init(Projectile projectile, GameEngine gameEngine)
        {
            return projectile.Parent.Lifetime * SizeMult + SizeOffset;
        }

    }
}