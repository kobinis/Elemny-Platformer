using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Init.InitFloat
{
    [Serializable]
    class InitFloatRotation : BaseInitFloat
    {
        public float Offset = 0;
        public float Mult = 1;


        public override float Init(Projectile projectile, GameEngine gameEngine)
        {
            return projectile.Rotation * Mult + Offset;
        }
    }
}
