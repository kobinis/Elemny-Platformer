using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils;
using XnaUtils.Framework.Graphics;
using XnaUtils.Graphics;

namespace SolarConflict.GameContent.Activities.Tests
{
    public enum AssistantState { Idle, WakingUp, Listening, Shutdown }



    class AdaActivity : Activity
    {        
        Texture2D pixel;
        Texture2D backPixel;
        float active;
        //float ping;
        //bool isPing;
        AssistantState state;
        Texture2D background;
        Vector2[] posVec;
        int n;
        float rad = 200;
        float coAmp = 1;
        float amp = 0;
        GameEngine particles;

        public AdaActivity()
        {
            pixel = TextureBank.Inst.GetTexture("add6");
            backPixel = TextureBank.Inst.GetTexture("glow128");
            state = AssistantState.Idle;
            background = TextureBank.Inst.GetTexture("window1");
            n = 600;
            posVec = new Vector2[n];
            Camera camera = new Camera();
            particles = new GameEngine(camera);
            GraphicsSettings.IsPostprocessing = false;
            //  webCam = new VideoCapture(ActivityManager.GraphicsDevice);

        }

        int counter;
        int timer = 0;
        public override void Update(InputState inputState)
        {
            particles.Update(inputState);
            particles.AddGameObject("SmallPlasmaTrail", Framework.FactionType.Neutral, particles.Camera.GetWorldPos(inputState.Cursor.Position));
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                ActivityManager.Inst.Back();
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D1))
                state = AssistantState.Idle;
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D2))
                state = AssistantState.WakingUp;

            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D3))
                state = AssistantState.Listening;
            if (inputState.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.D4))
                state = AssistantState.Shutdown;

            if (counter % 5 == 0)
            {
                if ((inputState.Cursor.Position - (ActivityManager.ScreenCenter - Vector2.UnitX * 400)).Length() < 300)
                {
                    state = AssistantState.WakingUp;
                    timer++;
                    if (timer > 450)
                    {
                        state = AssistantState.Listening;
                    }
                }
                else
                {
                    timer = 0;
                    state = AssistantState.Idle;
                }

                if (state == AssistantState.Listening)
                {
                    active = Math.Min(active + 0.05f, 1);
                    coAmp = FMath.MoveToTarget(coAmp, 1, 0.1f);
                    amp = FMath.MoveToTarget(amp, 0, 0.1f);
                }

                if (state == AssistantState.WakingUp)
                {
                    active = Math.Min(active + 0.05f, 1);
                    coAmp = FMath.MoveToTarget(coAmp, 0, 0.1f);
                    amp = FMath.MoveToTarget(amp, 1, 0.1f);
                }

                if (state == AssistantState.Idle)
                {
                    active = Math.Max(active - 0.05f, 0);
                    rad = FMath.MoveToTarget(rad, 200, 4);
                }

                if (state == AssistantState.Shutdown)
                {
                    active = Math.Max(active - 0.05f, 0);
                    rad = FMath.MoveToTarget(rad, 15, 3);
                }
            }
            counter++;
        }

        float time;
        public override void Draw(SpriteBatch sb)
        {

            sb.Begin();
            int w = background.Width;
            int h = background.Height;
            FMath.FitInsideBox(ref w, ref h, ActivityManager.ScreenWidth, ActivityManager.ScreenHeight);
            //new Rectangle(0, 0, w, h)
            sb.Draw(background, ActivityManager.ScreenRectangle, Color.White);

            // sb.Draw(webCam.Frame, ActivityManager.ScreenRectangle, Color.White);
            sb.End();
            time += 0.001f;
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);


            float widthMult = 0.8f;
            Random rand = new Random(1332);

            for (int i = 0; i < n; i++)
            {
                float angle = i * MathHelper.TwoPi / n;
                double radius = rad + active * Math.Cos(angle * 20 + time * 10) * rad * Math.Sin(time * 20) * 0.11f * (coAmp * Math.Max(Math.Sin(-angle - MathHelper.PiOver4), 0) + amp);
                double x = Math.Cos(angle) * radius;
                double y = Math.Sin(angle) * radius;

                //GraphicsUtils.DefaultSetPixel(sb, new Vector2((float)x, (float)y), Color.LightCyan);
                var pos = new Vector2((float)x, (float)y) + ActivityManager.ScreenCenter - Vector2.UnitX * 400;
                //float rotation = rand.NextFloat(0, MathHelper.TwoPi);// + time;
                sb.Draw(backPixel, pos, null, Color.Black, 1, new Vector2(backPixel.Width, backPixel.Height) * 0.5f, 0.3f * widthMult, SpriteEffects.None, 0);
                posVec[i] = pos;
            }
            float textScale = 3;
            string text = state.ToString();
            Vector2 size = Game1.menuFont.MeasureString(text) * textScale;
            sb.DrawString(Game1.menuFont, text, new Vector2(ActivityManager.ScreenWidth * 0.5f, 150), Color.Black, 0, size * 0.5f, textScale, SpriteEffects.None, 0);
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            for (int i = 0; i < n; i++)
            {
                float rotation = rand.NextFloat(0, MathHelper.TwoPi);// + time;
                sb.Draw(pixel, posVec[i], null, Color.White, rotation, new Vector2(pixel.Width, pixel.Height) * 0.5f, 1 * widthMult, SpriteEffects.None, 0);
            }
            sb.End();
            particles.Draw(sb);
        }

        public static Activity ActivityProvider(string parameters = "")
        {
            return new AdaActivity();
        }
    }
}
