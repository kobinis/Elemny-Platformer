using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Session.World.MissionManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    /// <summary>
    /// Shows location of ships from the same faction
    /// </summary>
    [Serializable]
    public class FactionShipsHudCmp : IHudComponent
    {
        private Sprite _arrowTexture;

        public FactionShipsHudCmp()
        {
            _arrowTexture = Sprite.Get("missionTarget");// "smallFuzzyArrow");
        }

        public void Update(Scene scene, Agent player)
        {

        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos)
        {
            if (player == null)
                return;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            Color color = new Color(0, 255, 0, 100);
            foreach (var target in scene.GameEngine._collideAllCheckList)
            {
                if (target.GetFactionType() == FactionType.Player & !scene.Camera.IsOnScreen(target))
                {                    
                    float distance = (float)Math.Round(GameObject.DistanceFromEdge(target.Position, player.Position, target.Size, player.Size) / 100);
                    Sprite sprite = _arrowTexture;
                    HudUtils.DrawArrow(scene.Camera, target.Position, target.Size, sprite, color, 0.9f);
                }
            }
            spriteBatch.End();
        }

        public Rectangle GetSize()
        {
            return new Rectangle();
        }


    }
}

