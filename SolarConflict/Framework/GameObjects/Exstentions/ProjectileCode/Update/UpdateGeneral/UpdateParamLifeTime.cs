using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public class UpdateParamLifeTime : BaseUpdate
    {
        public float Mult = 1;

        public UpdateParamLifeTime(float mult)
        {
            Mult = mult;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            projectile.Param += projectile.Lifetime * Mult;
        }
    }
}
