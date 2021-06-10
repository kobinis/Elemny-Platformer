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
    public class UpdateRotationOffsetHoming : BaseUpdate //check
    {
        public enum NoTargetRotationType { Projectile, Parent, Param}
        public NoTargetRotationType NoTargetRotation;
        public float RotationSpeed;
        //public float Fuzziness = 0; //ToDo
        public int UpdateTimeTick = 1;
        public int TimeToStart;
        public float OffsetMult;
        public float TimeMult;
        public ProjectileTargetType TargetType;
        public ProjectileTargetType SecounderyTargetType;

        public UpdateRotationOffsetHoming(float rotationSpeed = 0.8f, int timeToStart = 0, float offset = 0.4f, float timeMult = 0.1f, ProjectileTargetType targetType = ProjectileTargetType.Enemy)
        {
            TargetType = targetType;
            TimeMult = timeMult;
            OffsetMult = offset;
            RotationSpeed = rotationSpeed;
            TimeToStart = timeToStart;
            SecounderyTargetType = ProjectileTargetType.None;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            if (projectile.Lifetime > TimeToStart && projectile.Lifetime % UpdateTimeTick == 0)
            {
                GameObject target = projectile.GetProjectileTarget(gameEngine, TargetType);
                if(target == null)
                {
                    target = projectile.GetProjectileTarget(gameEngine, SecounderyTargetType);
                }
                float angle = projectile.Rotation;
                if (target != null)
                {                    
                    Vector2 diraction = target.Position - projectile.Position;                  
                    if (diraction.LengthSquared() > 0.001f)
                    {
                        angle = (float)Math.Atan2(diraction.Y, diraction.X);
                    }                                    
                }
                else
                {
                    switch (NoTargetRotation)
                    {
                        case NoTargetRotationType.Parent:
                            if (projectile.Parent != null && projectile.Parent.IsActive)
                            {
                                angle = projectile.Parent.Rotation;
                            }
                            break;
                        case NoTargetRotationType.Param:
                            angle = projectile.Param;
                            break;
                    }

                }
                angle += OffsetMult * (float)Math.Sin(projectile.Param + projectile.Lifetime * TimeMult + (projectile.GetHashCode() % 100)*2 );
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

