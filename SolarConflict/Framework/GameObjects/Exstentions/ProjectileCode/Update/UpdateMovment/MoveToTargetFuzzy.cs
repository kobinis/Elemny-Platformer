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
    public class MoveToTargetFuzzy : BaseUpdate
    {
        public ProjectileTargetType Target;
        public ProjectileTargetType SecondaryTarget;
        public float MaxSpeed;//if zero - instant
        public float Force;  //if force = 0, go with the target
        public bool SpeedWarping;
        public float Rad = 1000;

        public MoveToTargetFuzzy()
        {
        }

        public MoveToTargetFuzzy(ProjectileTargetType target, float force, float maxSpeed, bool speedWarping = false)
        {
            this.Target = target;
            this.MaxSpeed = maxSpeed;
            this.Force = force;
            this.SpeedWarping = speedWarping;
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            GameObject target = projectile.GetProjectileTarget(gameEngine, Target);
            if (target == null)
            {
                target = projectile.GetProjectileTarget(gameEngine, SecondaryTarget);
            }

            if (target != null)
            {
                if (Force != 0)
                {
                    Vector2 dif = target.Position - projectile.Position + FMath.ToCartesian(gameEngine.Rand.NextFloat() * Rad, gameEngine.Rand.NextFloat() * MathHelper.TwoPi);

                    if (dif != Vector2.Zero)
                    {
                        if (MaxSpeed == 0)
                        {
                            projectile.Position = target.Position;
                        }
                        else
                        {
                            float maxSpeed = MaxSpeed;
                            float force = Force;

                            if (SpeedWarping)
                            {
                                float distance = Math.Min(dif.Length(), 4500);
                                maxSpeed = Math.Max(maxSpeed * distance * 0.05f, MaxSpeed);
                                force = Math.Max(force * distance * 0.005f, Force);
                            }

                            dif.Normalize();
                            projectile.ApplyForce(dif * force, maxSpeed);

                            projectile.Position += projectile.Velocity; //remove
                            projectile.Velocity *= projectile.profile.VelocityInertia; //remove
                        }
                    }
                }
                else
                {
                    projectile.Position = target.Position;
                    projectile.Velocity = target.Velocity;
                    projectile.Velocity *= projectile.profile.VelocityInertia;
                }
            }
            else
            {
                projectile.Position += projectile.Velocity; //remove
                projectile.Velocity *= projectile.profile.VelocityInertia; //remove
            }
        }
    }
}
