using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    [Serializable]
    class PlayerHighlight : IHudComponent
    {
        private Sprite _highlightSprite;
        private float _sizeMult;

        public PlayerHighlight()
        {
            _highlightSprite = Sprite.Get("glow128");
            _sizeMult = 1f / _highlightSprite.Width * 2 * 1.5f;
        }

        public void Update(Scene scene, Agent player)
        {
            
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            if (player != null && !player.IsCloaked)
            {
                float size = player.Size * _sizeMult;
                scene.Camera.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                scene.Camera.CameraDraw(_highlightSprite, player.Position, 0, size, new Color(100, 100, 50));
                scene.Camera.SpriteBatch.End();
            }
        }

        public Rectangle GetSize()
        {
            return new Rectangle();
        }


    }
}
