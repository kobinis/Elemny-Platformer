using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaintPlay.XnaUtils.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PaintPlay.XnaUtils;
using PaintPlay;

namespace StarSingularity
{
    class InputVisualization
    {
        FxParticleEngine pEngine;
        ParticleProfile updateProfile;

        ParticleProfile handProfile;

        public InputVisualization()
        {
            pEngine = new FxParticleEngine();
            updateProfile = new ParticleProfile();
            updateProfile.maxLifetime = 3;
            updateProfile.dSize = -0.2f;
            updateProfile.scale = 0.5f;
            updateProfile.colorUpdater = new ColorFade();
            updateProfile.SetTexture(MyGraphics.GetTexture("glow"));

            handProfile = new ParticleProfile();
            handProfile.maxLifetime = 1;
            handProfile.dSize = -0f;
            handProfile.scale = 0.6f;
            handProfile.colorUpdater = new ColorFade();
            handProfile.SetTexture(MyGraphics.GetTexture("hand"));
        }

        public void Update(List<TouchState> touches)
        {
            foreach (TouchState touch in touches)
            {
                pEngine.AddParticle(updateProfile.MakeParticle(touch.Position, 1f));
            }

            float size = 1;
            Particle particle = handProfile.MakeParticle(XnaUtils.ActivityManager.Inst.InputState.Cursor.Position, size).SetColor(new Color(1f, 1f, 1f, 0.7f));
            pEngine.AddParticle(particle);
            pEngine.Update();
        }

        public void Draw()
        {
            MyGraphics.sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //" GestureUtils.pipe.gesture"
         /*   MyGraphics.sb.DrawString(MyGraphics.font, GestureUtils.pipe.gesture.ToString(), new Vector2(100, 100), Color.Red);
            MyGraphics.sb.DrawString(MyGraphics.font, GestureUtils.pipe.xx.ToString(), new Vector2(100, 150), Color.Red);*/
            pEngine.Draw();
            MyGraphics.sb.End();
        }
        
    }
}
