using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    //TODO: remove class
    [Serializable]
    public class UpdateSizeGrowMult : BaseUpdate
    {
        public float dSize;

        public UpdateSizeGrowMult()
        {

        }

        public UpdateSizeGrowMult(float dSize)
        {
            this.dSize = dSize;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            projectile.Size *= dSize;
        }
    }
}

