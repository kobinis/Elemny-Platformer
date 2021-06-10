using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using SolarConflict.Framework.Utils;
using XnaUtils.SimpleGui.Controllers;
using Microsoft.Xna.Framework;

namespace SolarConflict.Framework.Scenes.HudEngine.Components {
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    class MothershipStatusIndicator : IHudComponent { //TODO: add a ausio que when the mothership is low

        public const int MessageDuration =  2* Utility.FramesPerSecond;

        int _messageTime;
        
        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos) {
            if (_messageTime <= 0)
                return;

            spriteBatch.Begin();
            var control = new RichTextControl("Mothership under attack", null , true);
            control.TextColor = Color.Red;
            control.Position = new Vector2(spriteBatch.GraphicsDevice.Viewport.Width / 2, control.HalfSize.Y+5);
            control.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void Update(Scene scene, Agent player) {
            var mothership = scene.GameEngine.GetFaction(FactionType.Player).Mothership;

            //if ((mothership?.IsActive != true) || scene.Camera.IsOnScreen(mothership, 1f)) {
            //    // Either no need to report status (ship is onscreen), or no ship, no status
            //    _messageTime = 0;
            //    return;
            //}            

            --_messageTime;
            if (mothership != null && ( mothership.ControlSignal.HasFlag(ControlSignals.OnDamageToShield) || mothership.ControlSignal.HasFlag(ControlSignals.OnDamageToHull)))
                _messageTime = MessageDuration;
        }

        public Rectangle GetSize()
        {
            return new Rectangle();
        }
    }
}
