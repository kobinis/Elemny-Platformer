using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw
{
    /// <summary>
    /// Draws one sprite if the target is active and other if not
    /// </summary>
    [Serializable]
    class DrawIfTarget : BaseDraw
    {
        public BaseDraw DrawActive;
        public BaseDraw DrawNullOrNotActive;
        public ProjectileTargetType TargetType = ProjectileTargetType.LivePrimeAncestor;

        public override void Draw(Camera camera, Projectile projectile)
        {
            GameObject target = projectile.GetProjectileTarget(null, TargetType);
            if (projectile.Parent != null && projectile.Parent.IsActive && target != projectile)
            {
                DrawActive.Draw(camera, projectile);
            }
            else
            {
                DrawNullOrNotActive.Draw(camera, projectile);
            }
        }
    }
}
