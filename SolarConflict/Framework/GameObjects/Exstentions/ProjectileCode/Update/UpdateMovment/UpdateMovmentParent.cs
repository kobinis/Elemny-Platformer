using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;

namespace SolarConflict
{
    /// <summary>
    /// Rotate around or be on parent
    /// </summary>
    [Serializable]
    public class MoveWithParent : BaseUpdate
    {
       
        private float rotationSpeed;
        public float RotationSpeed
        {
            get { return MathHelper.ToDegrees(rotationSpeed); }
            set { rotationSpeed = MathHelper.ToRadians(value); }
        }

        private float rotationParam;

        public float RotationParam
        {
            get { return MathHelper.ToDegrees(rotationParam); }
            set { rotationParam = MathHelper.ToRadians(value); }
        }

        public float RefRotationMult;


        public MoveWithParent()
        {
        }

        public override void Update(Projectile projectile, float normalizedLifeTime, GameEngine gameEngine)
        {
            //Velocity X = radius
            //Velcity Y = radius

            //check what to do if parent is null or not active
            if (projectile.Parent != null)
            {

                if (projectile.Lifetime == 0)
                {
                    Vector2 relPos = (projectile.Position - projectile.Parent.Position);
                    projectile.Velocity.X = relPos.Length();
                    projectile.Velocity.Y = (float)Math.Atan2(relPos.Y, relPos.X);
                }

                float radius = projectile.Velocity.X;
                float angle = projectile.Velocity.Y;


                if (radius > 0)
                {
                    float rotationAngle = RefRotationMult * projectile.Parent.Rotation + angle + rotationSpeed * projectile.Lifetime / radius + rotationParam * projectile.Param;
                    // rotationParam * projectile.param *projectile.lifetime ???  //size
                    projectile.Position = projectile.Parent.Position + new Vector2(radius * (float)Math.Cos(rotationAngle), radius * (float)Math.Sin(rotationAngle));
                }
                else
                {
                    projectile.Position = projectile.Parent.Position;
                }
            }
            else
            {
                //move normaly ?????????????????????????
                projectile.Position += projectile.Velocity;
                projectile.Velocity *= projectile.profile.VelocityInertia;
            }
        }

    }
}
