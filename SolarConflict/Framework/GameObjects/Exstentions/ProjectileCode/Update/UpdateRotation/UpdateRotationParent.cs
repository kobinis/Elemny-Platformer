using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarConflict
{
    [Serializable]
    public class UpdateRotationParent : BaseUpdate //check
    {
      
        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            GameObject target = projectile.Parent;
            projectile.Rotation += projectile.RotationSpeed;
            projectile.RotationSpeed *= projectile.profile.RotationInertia; //move it out
            if (target != null)
            {
                projectile.Rotation = target.Rotation;
            }
        }

    }
}
