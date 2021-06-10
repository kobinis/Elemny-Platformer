using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using XnaUtils;
using XnaUtils.Graphics;

namespace SolarConflict
{
    //TODO: add different backgrounds,   make it pretty  Add fading layers, add stars and other background objects,
    //Add forground

    /// <summary>
    /// Displays Background and foreground
    /// </summary>
    [Serializable]
    public class Background //TODO: refractor
    {
        List<Sprite> _layers;
        Sprite _background;
        Sprite _forground;
        //List<Sprite> _rocks;
        Sprite starTexture;
        private bool _isShowRocks;
        public Color Color { get; set; }
        /// <summary>I guess this is initial tile size? This class wasn't really documented</summary>
        public float InitSize { get; set; }
        float time;

        public Background(int index, bool isShowRocks = true, Color? color = null, bool isRandom = false)
        {
            starTexture = Sprite.Get("dot7"); Sprite.Get("lightglow");
            index = index % 7;
            Sprite background = ("tile" + index.ToString()).ToSprite();
            if (isRandom && FMath.Rand.Next(2) == 0)
                background = ("tilea" + index.ToString()).ToSprite();
            Color = color ?? Color.White;
            _isShowRocks = isShowRocks;
            _background = background;
            _layers = new List<Sprite>();
            // _layers.Add(Sprite.Get("background1"));
            //_forground = Sprite.Get("background1");
            InitSize = 126;

        }

        public Background(string bgname, bool isShowRocks = true, Color? color = null, bool isRandom = false)
        {
            starTexture = Sprite.Get("lightglow");
            Sprite background = bgname.ToSprite();
            Color = color ?? Color.White;
            _isShowRocks = isShowRocks;
            _background = background;
            _layers = new List<Sprite>();
            // _layers.Add(Sprite.Get("background1"));
            //_forground = Sprite.Get("background1");
            InitSize = 126;

        }

        public void Draw(Camera camera, float speedMult = 1, bool showDust = true) //add Time
        {
            //  camera.UpdateMatrix();
            SpriteBatch sb = camera.SpriteBatch;
            Rectangle screenRectangle = ActivityManager.ScreenRectangle;
            float bgSpeed = 0.01f * speedMult;
            float cameraAspect = (float)screenRectangle.Width / (float)screenRectangle.Height;

            float zoom = 0.5f;// * (10 +camera.Zoom) / 10f;

            Vector4 sourceUV = new Vector4(camera.Position.X * bgSpeed, camera.Position.Y * bgSpeed, (screenRectangle.Width * cameraAspect) * zoom, screenRectangle.Height * zoom);

            Camera.BackgroundEffectSourceUV.SetValue(sourceUV);
            Camera.BackgroundEffectViewport.SetValue(new Vector2(screenRectangle.Width, screenRectangle.Height));

            sb.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, null, null, Camera.BackgroundEffect);
            sb.Draw(_background, screenRectangle, Color.White);

            sb.End();
            if (_isShowRocks && showDust)
            {
                sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearWrap, null, null, null);
                DrawDust(camera);
                sb.End();
            }
        }

        Camera newCamera = new Camera();
        public void DrawDust(Camera camera)
        {

            float size;
            for (int i = 6; i > 0; i--)
            {
                newCamera.Zoom = camera.Zoom / (i - 0.5f);
                newCamera.Position = camera.Position;
                size = i * 5f;// / camera.zoom; //

                //        camera.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                time += 0.0011f;
                float diff = 3.5f / (newCamera.Zoom);
                if (i > 1)
                    for (int x = 0; x <= 1; x++)
                    {
                        for (int y = 0; y <= 1; y++)
                        {

                            float rockSize = ((1 - (float)i * 0.05f) * 1f / newCamera.Zoom) * 0.8f;
                            float realSize = size * 2024 * 2; // 1024
                            Vector2 position = new Vector2(x * realSize + (float)Math.Floor(newCamera.Position.X / realSize) * realSize, y * realSize + (float)Math.Floor(newCamera.Position.Y / realSize) * realSize);
                            byte colValue = (byte)(Math.Min(Math.Max(255 - i * 10 - diff, 0), 255)); //Add twinkle and randomness
                            Color rockColor = new Color(colValue, colValue, colValue) * 0.8f;
                            Random rand = new Random(412232 + i * 232);
                            for (int n = 0; n < 200; n++)
                            {
                                float dx = (rand.Next((int)realSize) - realSize / 2);
                                float dy = (rand.Next((int)realSize) - realSize / 2);
                                Vector2 pos = position + new Vector2(dx, dy);
                                Sprite rock = starTexture; // _rocks[(index % count + count) % count];                                                                                                
                                //float rotation = ((n % 2) * 2 - 1) * time * 0.02f;                            
                                float rockRotation = (float)n / 200f * MathHelper.TwoPi;

                                newCamera.CameraDraw(starTexture, newCamera.GetScreenPos(pos), rockRotation + time, rockSize * newCamera.Zoom, rockColor); //DrawStar                                       
                            }

                        }

                    }

            }



            //int layerNum = 1;
            //int realSize = 2024;
            //for (int i = 0; i < layerNum; i++)
            //{
            //    byte colValue = (byte)(Math.Min(Math.Max(255 - i * 10, 0), 255)); //Add twinkle and randomness
            //    Color rockColor = new Color(colValue, colValue, colValue, colValue);
            //    Random rand = new Random(412232 + i * 232);

            //    for (int x = 0; x <= 1; x++)
            //    {
            //        for (int y = 0; y <= 1; y++)
            //        {
            //            Vector2 position = new Vector2(x * realSize + (float)Math.Floor(camera.Position.X / realSize) * realSize, y * realSize + (float)Math.Floor(camera.Position.Y / realSize) * realSize) - camera.Position;
            //            for (int n = 0; n < 200; n++)
            //            {
            //                float dx = (rand.Next((int)realSize) - realSize / 2);
            //                float dy = (rand.Next((int)realSize) - realSize / 2);
            //                Vector2 pos = position + ActivityManager.ScreenCenter + new Vector2(dx, dy) ;
            //                Sprite rock = starTexture; // _rocks[(index % count + count) % count];                                                                                                
            //                                           //float rotation = ((n % 2) * 2 - 1) * time * 0.02f;                            
            //                float rockRotation = (float)n * MathHelper.TwoPi * 2.1f;
            //                camera.SpriteBatch.Draw(starTexture.Texture,
            //                    pos, null, rockColor, rockRotation + time * 0.02f,
            //                    starTexture.Origin, 1, SpriteEffects.None, 0);
            //                //newCamera.CameraDraw(starTexture, pos, , rockSize, rockColor); //DrawStar                                       
            //            }
            //        }
            //    }
            //}



        }

        public void DrawForground(Camera camera)
        {
            int screenMaxSize = ActivityManager.ScreenRectangle.Width * 3;
            var rect = new Rectangle(0, 0, screenMaxSize, screenMaxSize);
            camera.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
            for (int dx = 0; dx < 2; dx++)
            {
                for (int dy = 0; dy < 2; dy++)
                {
                    rect.X = dx * screenMaxSize - FMath.Mod((int)camera.Position.X, screenMaxSize);
                    rect.Y = dy * screenMaxSize - FMath.Mod((int)camera.Position.Y, screenMaxSize);
                    camera.SpriteBatch.Draw(_forground.Texture, rect, new Color(255, 255, 255, 100));
                }
            }

        }

    }
}
