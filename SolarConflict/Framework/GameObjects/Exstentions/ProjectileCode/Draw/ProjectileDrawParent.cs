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
    /// Draws a projectile according to the size
    /// </summary>
    [Serializable]    
    public class ProjectileDrawParent : BaseDraw //target
    {
        private float displayRotation;

        public float DisplayRotation
        {
            get { return MathHelper.ToDegrees(displayRotation); }
            set { displayRotation = MathHelper.ToRadians(value); }
        }

        public ProjectileDrawParent()
        {
            displayRotation = 0;
        }

        public override void Draw(Camera camera, Projectile projectile)
        {
            if (projectile.Parent != null && projectile.Parent.GetSprite() != null)
            {                
                camera.CameraDraw(projectile.Parent.GetSprite(), projectile.Position, projectile.Rotation + displayRotation, projectile.Parent.GetDisplayScale(), projectile.ParticleColor);//mov
            }
        }

    }
}
