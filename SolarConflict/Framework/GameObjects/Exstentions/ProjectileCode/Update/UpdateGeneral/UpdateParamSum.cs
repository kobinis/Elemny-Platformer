using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public class UpdateParamSum : BaseUpdate
    {
        public float Delta;

        public UpdateParamSum(float delta)
        {
            Delta = delta;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            projectile.Param += Delta;
        }
    }
}
