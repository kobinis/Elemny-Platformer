using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment
{
    /// <summary>
    /// Sets the parent of the projectile to null
    /// </summary>
    [Serializable]
    public class UpdateParentNull : BaseUpdate
    {

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            projectile.Parent = null;    
        }
    }
}
