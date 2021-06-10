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
    /// Shows arrows to enemies and allys of screen
    /// </summary>
    [Serializable]
    public class TargetIndicator : IHudComponent
    {
        private Sprite _arrowTexture;

        public TargetIndicator()
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
            foreach (var target in scene.GameEngine._collideAllCheckList)
            {
                if ((target.GetObjectType() & GameObjectType.PotentialTarget) > 0 & !scene.Camera.IsOnScreen(target)  )
                {
                    Color color = target.GetFactionType() == FactionType.Player ? Color.Green : target.IsHostileToPlayer(scene.GameEngine) ? Color.Red : Color.Blue;
                    color.A = 200;
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
