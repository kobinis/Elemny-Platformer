using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateRotation
{
    [Serializable]
    class UpdateRotationLifetime : BaseUpdate
    {
        float mult;
        public UpdateRotationLifetime(float mult)
        {
            this.mult = mult;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            projectile.Rotation = projectile.Lifetime * mult;
        }
    }
}
