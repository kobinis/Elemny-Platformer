using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils;

namespace SolarConflict
{
    /// <summary>
    /// Draws a projectile to face target
    /// </summary>
    [Serializable]
    public class DrawRotationParent : BaseDraw
    {
        private float angleOffset;

        public float AngleOffset
        {
            get { return MathHelper.ToDegrees(angleOffset); }
            set { angleOffset = MathHelper.ToRadians(value); }
        }


        public DrawRotationParent()
        {
        }

        public DrawRotationParent(float angle)
        {
            AngleOffset = angle;
        }

        public override void Draw(Camera camera, Projectile projectile)
        {
            float rotation = projectile.Rotation;
            if (projectile.Parent != null) //and maybe active
            {
                Vector2 relPos = projectile.Position - projectile.Parent.Position;
                rotation = (float)Math.Atan2(relPos.Y, relPos.X) + angleOffset;
            }

            camera.CameraDraw(projectile.profile.Sprite, projectile.Position, rotation, projectile.Size * projectile.profile.ScaleMult, projectile.color);
        }

    }
}
