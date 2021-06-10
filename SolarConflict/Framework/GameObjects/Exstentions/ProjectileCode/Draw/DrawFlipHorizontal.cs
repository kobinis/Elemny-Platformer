using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict
{
    /// <summary>
    /// Draws a projectile according to the size
    /// </summary>
    [Serializable]
    public class DrawFlipHorizontal : BaseDraw
    {
        public Sprite Sprite;

        public DrawFlipHorizontal(Sprite sprite)
        {
            Sprite = sprite;
        }

        public override void Draw(Camera camera, Projectile projectile)
        {
            Sprite sprite = Sprite;
            if (sprite == null)
            {
                sprite = projectile.profile.Sprite;
            }
            camera.CameraDraw(sprite, projectile.Position, projectile.Rotation, projectile.Size * projectile.profile.ScaleMult, projectile.color, SpriteEffects.FlipHorizontally);
        }

    }
}

