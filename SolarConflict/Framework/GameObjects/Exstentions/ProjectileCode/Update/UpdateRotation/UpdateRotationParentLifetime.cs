using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateRotation
{
    [Serializable]
    class UpdateRotationParentLifetime : BaseUpdate
    {
        float mult;
        public UpdateRotationParentLifetime(float mult)
        {
            this.mult = mult;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            if(projectile.Parent != null)
                projectile.Rotation = projectile.Parent.Lifetime * mult;
        }
    }
}

