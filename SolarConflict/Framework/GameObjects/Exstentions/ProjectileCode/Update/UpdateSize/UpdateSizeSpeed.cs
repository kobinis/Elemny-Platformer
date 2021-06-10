using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    //TODO: remove class
    [Serializable]
    public class UpdateSizeSpeed : BaseUpdate
    {
        public float offset;
        public float mult;

        public UpdateSizeSpeed(float mult, float offset)
        {
            this.mult = mult;
            this.offset = offset;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            projectile.Size = projectile.Velocity.Length()*mult + offset;
        }
    }
}

