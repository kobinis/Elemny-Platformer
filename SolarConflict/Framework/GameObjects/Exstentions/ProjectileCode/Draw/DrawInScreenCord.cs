using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw
{
    [Serializable]
    class DrawInScreenCord: BaseDraw
    {
        public override void Draw(Camera camera, Projectile projectile)
        {
            Sprite sprite = projectile.GetSprite();            
            camera.SpriteBatch.Draw(sprite.Texture, projectile.Position + ActivityManager.ScreenSize * 0.5f, null, projectile.color, projectile.Rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), projectile.Size * projectile.profile.ScaleMult, SpriteEffects.None, 0);
        }
    }
}
