using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils;

namespace SolarConflict
{
    [Serializable]
    public class UpdateRotationForward : BaseUpdate
    {
        public float BaseRotation;
        public float RotationSpeed;


        public UpdateRotationForward()
        {            
        }

        public UpdateRotationForward(float rotationSpeed)
        {
            this.RotationSpeed = rotationSpeed;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            if (projectile.Velocity != Vector2.Zero)
            {
                float angle = (float)Math.Atan2(projectile.Velocity.Y, projectile.Velocity.X) + BaseRotation;
                if (RotationSpeed == 0)
                {                    
                    projectile.Rotation = angle;
                }
                else
                {                    
                    float angleDiff = (float)FMath.AngleDiff(angle, projectile.Rotation);
                    if (Math.Abs(angleDiff) <= RotationSpeed)
                    {
                        projectile.Rotation = angle;
                    }
                    projectile.Rotation += RotationSpeed * (float)FMath.DegSign(angle - projectile.Rotation);
                }
            }
        }

    }
}
