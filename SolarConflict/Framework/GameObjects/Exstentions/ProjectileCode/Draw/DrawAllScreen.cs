using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;

namespace SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode.Draw
{
    /// <summary>
    /// Draws overlay all over the screen
    /// </summary>
    [Serializable]
    public class DrawAllScreen : BaseDraw
    {
        public Color OverlayColor { get; set; }
        public float AlphaOffset = 0;
        public DrawAllScreen()
        {
            OverlayColor = Color.White;
        }
        public override void Draw(Camera camera, Projectile projectile)
        {
            Vector2 diff = projectile.Position - camera.Position;
            float distance = diff.Length() ;
            float alpha = MathHelper.Clamp(AlphaOffset + 1f - distance / (projectile.Size * 1.5f), 0, 1);
            Color color = OverlayColor;
            color.A = (byte)(alpha * 255);
            if (color.A > 0)
            {
                var pos = camera.GetWorldPos(Vector2.Zero);
                Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, (int)(ActivityManager.ScreenRectangle.Width / camera.Zoom) + 1, (int)(ActivityManager.ScreenRectangle.Width / camera.Zoom) + 1);
                //ActivityManager.ScreenRectangle()
                camera.SpriteBatch.Draw(projectile.profile.Sprite.Texture, rect, color);
               //TODO: possibly fix it
            }
        }
    }
}
