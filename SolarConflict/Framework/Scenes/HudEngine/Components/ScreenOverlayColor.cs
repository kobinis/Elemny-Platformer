using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.Graphics;
using XnaUtils;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    [Serializable]
    class ScreenOverlayColor : IHudComponent
    {
        private Sprite overlaySprite;

        public ScreenOverlayColor()
        {
            overlaySprite = Sprite.Get("blank");
        }

        public void Update(Scene scene, Agent player)
        {        
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            spriteBatch.Begin();
            if (scene.GameEngine.ScreenOverlayColor != Color.Transparent)
            {
                spriteBatch.Draw(overlaySprite, ActivityManager.ScreenRectangle, scene.GameEngine.ScreenOverlayColor);
            }
            spriteBatch.End();
        }

        public Rectangle GetSize()
        {
            return new Rectangle();
        }

    }
}
