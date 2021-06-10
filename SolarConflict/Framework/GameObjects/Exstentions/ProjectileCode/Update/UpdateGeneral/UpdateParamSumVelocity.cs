using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public class UpdateParamSumVelocity : BaseUpdate
    {
        public float MinValue = 0f;
        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            float value = projectile.Velocity.Length();
            projectile.Param += value;                                    
        }
    }
}
