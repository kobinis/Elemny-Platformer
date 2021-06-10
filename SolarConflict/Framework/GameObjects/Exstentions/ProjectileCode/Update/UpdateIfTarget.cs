using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Update.UpdateMovment
{
    /// <summary>
    /// Provide a choice between two different updates according to target state
    /// </summary>
    [Serializable]
    public class UpdateIfTarget : BaseUpdate
    {
        public ProjectileTargetType TargetType = ProjectileTargetType.LivePrimeAncestor;
        public BaseUpdate UpdateParentActive;
        public BaseUpdate UpdateParentNullOrNotActive;

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            GameObject target = projectile.GetProjectileTarget(gameEngine, TargetType);
            if (target != null && target.IsActive && target != projectile)
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
