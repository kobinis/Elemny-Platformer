using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils;
using XnaUtils.Graphics;
using Microsoft.Xna.Framework;

namespace SolarConflict
{
    [Serializable]
    class ProjectileDrawRotateWithTime : BaseDraw  
    {
        public float RotationMult1 { get; set; }
        public float RotationMult2 { get; set; }
        public float RandomMult { get; set; }
        public float DisplayScaleMult1 { get; set; }
        public float DisplayScaleMult2 { get; set; }
        private Sprite _texture1;
        private Sprite _texture2;
        
        public ProjectileDrawRotateWithTime(float rotationMult1, float rotationMult2, string textureId1, string textureId2 = null, float displayScaleMult1 = 0, float displayScaleMult2 = 0, float randomMult = MathHelper.TwoPi)
        {
            _texture1 = Sprite.Get(textureId1);
            _texture2 = Sprite.Get(textureId2);
            if (displayScaleMult1 > 0)
            {
                DisplayScaleMult1 = displayScaleMult1;
            }
            else
            {
                DisplayScaleMult1 = ProjectileProfile.WidthToScale(Math.Min(_texture1.Width, _texture1.Height));
            }

            if (displayScaleMult2 > 0)
            {
                DisplayScaleMult2 = displayScaleMult2;
            }
            else
            {
                if (textureId2 != null) { DisplayScaleMult2 = ProjectileProfile.WidthToScale(Math.Min(_texture2.Width, _texture2.Height)); }
            }
            
            this.RotationMult1 = rotationMult1;
            this.RotationMult2 = rotationMult2;
            RandomMult = randomMult;
        }

        public override void Draw(Camera camera, Projectile projectile)
        {          
            float offset = (projectile.GetHashCode() % 100) * 0.01f;            
            camera.CameraDraw(_texture1, projectile.Position, projectile.Rotation + projectile.Lifetime * RotationMult1 + offset * RandomMult, projectile.Size * DisplayScaleMult1, projectile.color);            
            if(_texture2 != null)
            {
                camera.CameraDraw(_texture2, projectile.Position, projectile.Rotation + projectile.Lifetime * RotationMult2 + offset * RandomMult, projectile.Size * DisplayScaleMult2, projectile.color);
            }            
        }
    }
}
