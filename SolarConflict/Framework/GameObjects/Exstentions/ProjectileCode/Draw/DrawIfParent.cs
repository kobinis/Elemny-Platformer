using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw
{
    /// <summary>
    /// Draws one sprite if the parent is active and other if not
    /// </summary>
    [Serializable]
    class DrawIfParent : BaseDraw
    {
        public BaseDraw DrawParentActive;
        public BaseDraw DrawParentNullOrNotActive;

        public override void Draw(Camera camera, Projectile projectile)
        {
            if (projectile.Parent != null && projectile.Parent.IsActive)
            {
                DrawParentActive.Draw(camera, projectile);
            }
            else
            {
                DrawParentNullOrNotActive.Draw(camera, projectile);
            }
        }
    }
}
