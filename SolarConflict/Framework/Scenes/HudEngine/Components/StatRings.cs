using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XnaUtils.Graphics;
using XnaUtils;
using System.Diagnostics;
using SolarConflict.Framework.Utils;

namespace SolarConflict.Framework.Scenes.HudEngine.Components {
    [Serializable]
    class StatRings : IHudComponent {        
        Sprite _dot;
        Vector2[] _innerRingPositions;
        Vector2[] _outerRingPositions;        

        public StatRings(int numInnerRingDots = 10, float innerRingRadius = 30f, int numOuterRingDots = 6, float outerRingRadius = 40f) {
            _dot = Sprite.Get("dot7");

            _innerRingPositions = new Ring(innerRingRadius)
                .Transforms(numInnerRingDots).Select(t => t.Position).ToArray();
            _outerRingPositions = new Ring(outerRingRadius)
                .Transforms(numOuterRingDots).Select(t => t.Position).ToArray();            
        }

        public void Draw(SpriteBatch spriteBatch, Scene scene, Agent player, Vector2 pos) {
            if (player == null)
                return;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);

            // Draw energy ring            
            DrawRing(spriteBatch, scene.InputState.Cursor.Position, _innerRingPositions, player.GetMeter(MeterType.Energy), new Color(255,255,0,250));
            DrawCompositeRing(spriteBatch, scene.InputState.Cursor.Position, _outerRingPositions, player.GetMeter(MeterType.Hitpoints), player.GetMeter(MeterType.Shield),
                Color.Green, Color.Blue);
            spriteBatch.End();
        }

        /// <summary>Draw a ring representing a meter</summary>
        /// <remarks>Draws dots in a fraction of the given positions proportional to meterValue/meterMaxValue</remarks>
        void DrawRing(SpriteBatch batch, Vector2 position, Vector2[] dotLocalPositions, Meter meter, Color color) {
            position -= new Vector2(_dot.Width, _dot.Height) * 0.5f;

            if (meter.MaxValue <= 0f)
                return;

            var ratio = meter.Value / meter.MaxValue;

            var numDots = (int)Math.Min( Math.Ceiling(dotLocalPositions.Length * ratio), dotLocalPositions.Length);
            if (numDots > 0)
            {
                // Draw dots
                dotLocalPositions.Take(numDots - 1).Do(p => batch.Draw(_dot, position + p, color));

                // Draw last dot with alpha
                var alphaValue = ratio == 1f ? 1f : (float)(dotLocalPositions.Length * ratio - Math.Floor(dotLocalPositions.Length * ratio));
                color.A = (byte)(alphaValue * color.A);

                batch.Draw(_dot, position + dotLocalPositions.ElementAt(numDots - 1), color);
            }
        }

        void DrawCompositeRing(SpriteBatch batch, Vector2 position, Vector2[] dotLocalPositions, Meter meter1, Meter meter2, Color color1, Color color2) {
            if (meter1.MaxValue + meter2.MaxValue<= 0f)
                return;
            // KLUDGE: a meter's Value can exceed MaxValue, but we generally pretend it doesn't (actual value is Min(Value, MaxValue). Do so here, too
            var value1 = Math.Min(meter1.Value, meter1.MaxValue);
            var value2 = Math.Min(meter2.Value, meter2.MaxValue);
            position -= new Vector2(_dot.Width, _dot.Height) * 0.5f;
            var ratio = (value1 + value2) / (meter1.MaxValue + meter2.MaxValue);
            var numDots = (int)Math.Min(Math.Ceiling(dotLocalPositions.Length * ratio), dotLocalPositions.Length );
            if (numDots > 0)
            {
                var dotsForFirstMeter = (int)Math.Min(Math.Ceiling(dotLocalPositions.Length * (value1 / (meter1.MaxValue + meter2.MaxValue))), dotLocalPositions.Length);

                // Draw dots
                dotLocalPositions.Take(numDots - 1).Enumerate().Do(pair => batch.Draw(_dot, position + pair.Item2,
                    //((float)pair.Item1) / dotLocalPositions.Length <= ratio ? color1 : color2));
                    pair.Item1 < dotsForFirstMeter ? color1 : color2));

                // Draw last dot with alpha
                var color = value2 > 0f ? color2 : color1; // note that this is incorrect in the edge case of the second meter being too small to have any dots at all
                var alphaValue = ratio == 1f ? 1f : (float)(dotLocalPositions.Length * ratio - Math.Floor(dotLocalPositions.Length * ratio));
                color.A = (byte)(alphaValue * color.A);
                batch.Draw(_dot, position + dotLocalPositions.ElementAt(numDots - 1), color);
            }
        }

        public void Update(Scene scene, Agent player) { }

        public Rectangle GetSize()
        {
            return new Rectangle();
        }
    }
}
