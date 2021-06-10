using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.GameObjects.Exstentions.AgentGameObject.Systems.Misc;
using SolarConflict.Framework.Utils;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using XnaUtils.SimpleGui.Controllers;

namespace SolarConflict.Framework.Scenes.HudEngine.Components
{
    /// <summary>
    /// Shows the hitpoint and shield bar of near by agents not including player
    /// </summary>
    [Serializable]
    public class RespawnIndicator : IHudComponent
    {
        private int _selectedSlotIndex;
        private int _respawnTime;

        public void Update(Scene scene, Agent agent)
        {
            _respawnTime =0;

            var mothership = scene.GetPlayerFaction().Mothership;

            if (mothership == null)// No mothership
                return;
      
            var fleetSystem = mothership.GetSystem<FleetSystem>();

            if (fleetSystem == null) // No respawn system
                return;

            if (scene.PlayerAgent != null)
            {
                // Keep track of which slot's ship the player's selected
                _selectedSlotIndex = fleetSystem.FleetSlots.IndexOfFirst(s => s.Agent == scene.PlayerAgent);
            }

            if (_selectedSlotIndex >= 0)
            {
                var slot = fleetSystem.FleetSlots[_selectedSlotIndex];

                if (slot.Agent?.IsActive == false)
                {
                    _respawnTime = Math.Max(0, slot.Cooldown);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos) {          
            if (_respawnTime <= 0)
                return;

            spriteBatch.Begin();

            var text = $"{"Redeploy in: "} {(((float)_respawnTime) / Utility.FramesPerSecond).ToString("G1")} seconds";
            var timerControl = new RichTextControl(text);
            timerControl.Position = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, 200);
            timerControl.Draw(spriteBatch);            
            spriteBatch.End();          
        }

        public Rectangle GetSize()
        {
            return new Rectangle();
        }

    }
}
