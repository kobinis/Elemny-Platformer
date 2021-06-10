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
    public class MoveForward : BaseUpdate
    {
        
        public MoveForward()
        {
        }

        public MoveForward(float force, float maxSpeed)
        {
            this.MaxSpeed = maxSpeed;
            this.Force = force;
        }

        public float MaxSpeed = 10; //topspeed        
        public float Force;//acceleration; if force is zero constant speed
        
        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {

            Vector2 rotatedForce = FMath.ToCartesian(Force, projectile.Rotation);

            projectile.ApplyForce(rotatedForce, MaxSpeed);            
            projectile.Position += projectile.Velocity; //not good
            projectile.Velocity *= projectile.profile.VelocityInertia; //not good remove
        }

    }
}
