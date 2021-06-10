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
    public class Background2 //TODO: refractor
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
        /// <summary>If true, background will pan/scroll as though it's on the same plane as the player ship, else it will pretend to actually be some
        /// distance in the background.</summary>        
        public bool TandemScroll;// = false;


        public Background2(int index, bool isShowRocks = true, Color? color = null, bool isRandom = false)
        {
            starTexture = Sprite.Get("griddot");
            index = index % 7;
            Sprite background = ("tile" + index.ToString()).ToSprite();
            if (isRandom && FMath.Rand.Next(2) == 0)
                background = ("tilea" + index.ToString()).ToSprite();
            Color = color ?? Color.White;
            _isShowRocks = isShowRocks;
            _background = background;
            _layers = new List<Sprite>();
            _layers.Add(Sprite.Get("background1"));
            _forground = Sprite.Get("background1");
            InitSize = 126;
            TandemScroll = false;
        }

        float time;

        public void Draw(Camera camera) //add Time
        {
            time += 1f;

            camera.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Rectangle backgroundRect = new Rectangle(0,0, camera.screenSize.Width, camera.screenSize.Width);
            // camera.spriteBatch.Draw(background, backgroundRect, Color.White);

            Camera newCamera = new Camera();

            newCamera.Zoom = TandemScroll ? camera.Zoom : 0.01f;
            newCamera.Position = camera.Position;
            float size = InitSize; //* newCamera.zoom;          

            if (TandemScroll)
                newCamera.Zoom = camera.Zoom;

            var actualTileSize = size * _background.Width;
            var numTilesHorizontal = 2;// ((int)Math.Ceiling(((float)camera.ScreenWidth) / size)) / 2;
            var numTilesVertical = 2;// ((int)Math.Ceiling(((float)camera.ScreenHeight) / size)) / 2;
            for (int x = -numTilesHorizontal; x <= numTilesHorizontal; x++)
                for (int y = -numTilesVertical; y <= numTilesVertical; y++)
                {
                    float realSize = size * _background.Width;
                    Vector2 position = new Vector2(x * realSize + (float)Math.Floor(newCamera.Position.X / realSize) * realSize, y * realSize + (float)Math.Floor(newCamera.Position.Y / realSize) * realSize);
                    newCamera.CameraDraw(_background, position, 0, size, Color);
                }

            for (int i = 6; i > 0; i--)
            {
                newCamera.Zoom = camera.Zoom / i;
                newCamera.Position = camera.Position;
                size = i * 5f;// / camera.zoom; //







                if (_layers != null && i < _layers.Count)
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {



                            byte value = (byte)Math.Max(100 - i * 30, 0);
                            Color color = new Color(value, value, value, value);
                            float realSize = size * _layers[i - 1].Width;
                            Vector2 position = new Vector2(x * realSize + (float)Math.Floor(newCamera.Position.X / realSize) * realSize, y * realSize + (float)Math.Floor(newCamera.Position.Y / realSize) * realSize);
                            newCamera.CameraDraw(_layers[i - 1], position, 0, size, color);

                            int posX = (int)((Math.Round(position.X) - Math.Floor(position.X / size) * size) + x * size - size);
                            int posY = (int)((Math.Round(position.Y) - Math.Floor(position.Y / size) * size) + y * size - size);
                            Rectangle layerRect = new Rectangle((int)posX, (int)posY, (int)size, (int)size);
                            camera.SpriteBatch.Draw(_layers[i - 1], layerRect, Color.White);
                            //float alpha = */

                        }

                    }
                camera.SpriteBatch.End();

                camera.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

                float diff = 3.5f / (newCamera.Zoom);
                if (i > 1 && _isShowRocks)
                    for (int x = 0; x <= 1; x++)
                    {
                        for (int y = 0; y <= 1; y++)
                        {

                            float rockSize = (1 - (float)i * 0.05f) * 1f / newCamera.Zoom;
                            float realSize = size * 2024 * 2; // 1024
                            Vector2 position = new Vector2(x * realSize + (float)Math.Floor(newCamera.Position.X / realSize) * realSize, y * realSize + (float)Math.Floor(newCamera.Position.Y / realSize) * realSize);
                            byte colValue = (byte)(Math.Min(Math.Max(255 - i * 10 - diff, 0), 255)); //Add twinkle and randomness
                            Color rockColor = new Color(colValue, colValue, colValue, colValue);
                            Random rand = new Random(412232 + i * 232);
                            for (int n = 0; n < 200; n++)
                            {
                                float dx = (rand.Next((int)realSize) - realSize / 2);
                                float dy = (rand.Next((int)realSize) - realSize / 2);
                                Vector2 pos = position + new Vector2(dx, dy);
                                Sprite rock = starTexture; // _rocks[(index % count + count) % count];                                                                                                
                                //float rotation = ((n % 2) * 2 - 1) * time * 0.02f;                            
                                float rockRotation = (float)n * MathHelper.TwoPi * 2.1f;
                                newCamera.CameraDraw(starTexture, pos, rockRotation + time * 0.02f, rockSize, rockColor); //DrawStar                                       
                            }

                        }

                    }

            }

            camera.SpriteBatch.End();




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
            camera.SpriteBatch.End();
        }



    }
}
