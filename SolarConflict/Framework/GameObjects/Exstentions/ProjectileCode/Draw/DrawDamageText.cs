using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using SolarConflict.Framework.GameObjects.Exstentions.ProjectileCode;
using XnaUtils;
using Microsoft.Xna.Framework.Graphics;

namespace SolarConflict
{
    /// <summary>
    /// Draws the content of param as text
    /// </summary>
    [Serializable]    
    public class DrawDamageText : BaseDraw
    {
                
        public override void Draw(Camera camera, Projectile projectile)
        {
            Color col = new Color(255, 85, 85);
            if (projectile.GetFactionType() == Framework.FactionType.Player) 
                col = new Color(255,255,85);
            col.A = projectile.ParticleColor.A; //TOTO: check diffrance from color
            float scale = Math.Min( Math.Max( projectile.Param / 500f, 1f), 3.5f) / camera.Zoom ;
            string text = Math.Round(projectile.Param).ToString();
            Vector2 center = Game1.menuFont.MeasureString(text) * 0.5f;
            Vector2 position = projectile.Position;
            camera.SpriteBatch.DrawString(Game1.orbitron12, text, position - Vector2.UnitY*projectile.Lifetime, col,
                0, center, scale, SpriteEffects.None, 0);
        }
    }
}
