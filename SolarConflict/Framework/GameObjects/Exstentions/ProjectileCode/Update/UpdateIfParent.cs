using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment
{
    /// <summary>
    /// Provide a choice between two different updates according to parent state
    /// </summary>
    [Serializable]
    public class UpdateIfParent : BaseUpdate
    {
        public BaseUpdate UpdateParentActive;
        public BaseUpdate UpdateParentNullOrNotActive;

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            if(projectile.Parent != null && projectile.Parent.IsActive)
            {
                UpdateParentActive.Update(projectile, normalizedLifeTime, gameEngine);
            }
            else
            {
                UpdateParentNullOrNotActive.Update(projectile, normalizedLifeTime, gameEngine);
            }
        }
    }
}
