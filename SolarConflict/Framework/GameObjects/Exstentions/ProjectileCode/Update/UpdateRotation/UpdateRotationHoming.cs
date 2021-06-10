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
    public class UpdateRotationHoming : BaseUpdate //check
    {
        public float RotationSpeed;
        //public float Fuzziness = 0; //ToDo
        public int UpdateTimeTick = 2;
        public int TimeToStart;
        ProjectileTargetType TargetType;

        public UpdateRotationHoming(float rotationSpeed = 0.08f, int timeToStart = 0, ProjectileTargetType targetType = ProjectileTargetType.Enemy)
        {
            TargetType = targetType;
            RotationSpeed = rotationSpeed;
            TimeToStart = timeToStart;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            if (projectile.Lifetime > TimeToStart && projectile.Lifetime % UpdateTimeTick == 0)
            {
                GameObject target = projectile.GetProjectileTarget(gameEngine, TargetType);

                if (target != null)
                {
                    Vector2 diraction = target.Position - projectile.Position;
                    if (diraction.LengthSquared() > 0.01f)
                    {
                        float angle = (float)Math.Atan2(diraction.Y, diraction.X);
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
}
