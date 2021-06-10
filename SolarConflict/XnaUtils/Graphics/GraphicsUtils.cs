using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaUtils.Graphics;

namespace XnaUtils.Framework.Graphics
{
    //public delegate void SetPixel(int x, int y, int color);
    //public delegate Color GetPixel(int x, int y);


    public delegate void SetPixel(SpriteBatch sb, Vector2 point, Color color);
    

    public class GraphicsUtils
    {
        private static Texture2D defaultPixelTexture;

        public static void Init()// Texture pixelTexture)
        {
            defaultPixelTexture = TextureBank.Inst.GetTexture("griddot");
        }       

        public static void DrawBackground(Texture2D cover, SpriteBatch sb, Color? color = null)
        {
            color = color ?? Color.White;

            float scale = FMath.FindBigScale(new Vector2(cover.Width, cover.Height), ActivityManager.ScreenSize);
            Vector2 newSize = new Vector2(cover.Width * scale, cover.Height * scale);
            int ypos = (int)(ActivityManager.ScreenSize.Y - newSize.Y) / 2;
            int xpos = (int)(ActivityManager.ScreenSize.X - newSize.X) / 2;
            Rectangle rect = new Rectangle(xpos, ypos, (int)newSize.X, (int)newSize.Y);
            sb.Draw(cover, rect, color.Value);
        }

        /******************** 9 Slice - move code **********************/
        //public static Rectangle[] Create9SlicePatches(Rectangle rectangle, int horizontalPadding = 3, int verticalPadding = 3)
        //{
        //    return Create9SlicePatches(rectangle, horizontalPadding, horizontalPadding, verticalPadding, verticalPadding);
        //}

        public static Rectangle[] Create9SlicePatches(Rectangle rectangle, int leftPadding, int rightPadding, int topPadding, int bottomPadding)
        {            
            var x = rectangle.X;
            var y = rectangle.Y;
            var w = rectangle.Width;
            var h = rectangle.Height;
            var middleWidth = w - leftPadding - rightPadding;
            var middleHeight = h - topPadding - bottomPadding;
            var bottomY = y + h - bottomPadding;
            var rightX = x + w - rightPadding;
            var leftX = x + leftPadding;
            var topY = y + topPadding;
            var patches = new[]
            {
                new Rectangle(x,      y,        leftPadding,  topPadding),      // top left
                new Rectangle(leftX,  y,        middleWidth,  topPadding),      // top middle
                new Rectangle(rightX, y,        rightPadding, topPadding),      // top right
                new Rectangle(x,      topY,     leftPadding,  middleHeight),    // left middle
                new Rectangle(leftX,  topY,     middleWidth,  middleHeight),    // middle
                new Rectangle(rightX, topY,     rightPadding, middleHeight),    // right middle
                new Rectangle(x,      bottomY,  leftPadding,  bottomPadding),   // bottom left
                new Rectangle(leftX,  bottomY,  middleWidth,  bottomPadding),   // bottom middle
                new Rectangle(rightX, bottomY,  rightPadding, bottomPadding)    // bottom right
            };
            return patches;
        }
        
        public static void Draw9Slice(Texture2D texture, SpriteBatch spriteBatch, Rectangle rectangle, Color color, Rectangle[] _sourcePatches, int horizontalPadding = 10, int verticalPadding = 10)
        {
            var destinationPatches = Create9SlicePatches(rectangle, _sourcePatches[0].Width, _sourcePatches[8].Width, _sourcePatches[0].Height, _sourcePatches[8].Height);
            for (var i = 0; i < _sourcePatches.Length; i++)
            {
                spriteBatch.Draw(texture, sourceRectangle: _sourcePatches[i], destinationRectangle: destinationPatches[i], color: color);
            }
        }



        public static void Line(SpriteBatch sb, Vector2 point1, Vector2 point2, Color color, float rad, SetPixel setPixel)
        {            
            Vector2 diff = point2 - point1;            
            if (diff != Vector2.Zero)
            {
                float dis = diff.Length();
                int n = (int)Math.Ceiling(dis / rad * 2);
                Vector2 step = diff /(float) n;                                
                for (int i = 0; i < n; i++)
                {
                    Vector2 position = point1 + i * step;
                    setPixel(sb, position, color);
                }
            }
            else
            {
                setPixel(sb, point1, color);
            }
        }

        public static void DefaultSetPixel(SpriteBatch sb, Vector2 point, Color color)
        {
            sb.Draw(defaultPixelTexture, point, color);
        }

        /*
        public void Line(int x1, int y1, int x2, int y2, float Mult,  Color color , SetPixel setPixel)
        {

        }

        public void Circle(int x, int y, int rad, Color color, SetPixel setPixel)
        {

        }

        public void FloodFill(int x, int y, Color color, Color TempColor, SetPixel setPixel, GetPixel getPixel)
        {

        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Color HsvToRgb(float h, float s, float v)
        {
            int hi = (int)Math.Floor(h / 60.0) % 6;
            float f = (h / 60f) - (float)Math.Floor(h / 60f);

            float p = v * (1f - s);
            float q = v * (1f - (f * s));
            float t = v * (1f - ((1f - f) * s));

            Color ret;
            switch (hi)
            {
                case 0:
                    ret = new Color(v, t, p);
                    break;
                case 1:
                    ret = new Color(q, v, p);
                    break;
                case 2:
                    ret = new Color(p, v, t);
                    break;
                case 3:
                    ret = new Color(p, q, v);
                    break;
                case 4:
                    ret = new Color(t, p, v);
                    break;
                case 5:
                    ret = new Color(v, p, q);
                    break;
                default:
                    ret = Color.Black;
                    break;
            }
            return ret;
        }


        public static void RgbToHsv(byte src_r, byte src_g, byte src_b, out float h, out float s, out float v)
        {
            float r = src_r / 255.0f;
            float g = src_g / 255.0f;
            float b = src_b / 255.0f;

            // h:0-360.0, s:0.0-1.0, v:0.0-1.0

            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));

            v = max;

            if (max == 0.0f) {
                s = 0;
                h = 0;
            }
            else if (max - min == 0.0f) {
                s = 0;
                h = 0;
            }
            else {
                s = (max - min) / max;

                if (max == r) {
                    h = 60 * ((g - b) / (max - min)) + 0;
                }
                else if (max == g) {
                    h = 60 * ((b - r) / (max - min)) + 120;
                }
                else {
                    h = 60 * ((r - g) / (max - min)) + 240;
                }
            }

            if (h< 0) h += 360.0f;
        }

        private static byte ApplyAlpha(byte color, byte alpha)
        {
            var fc = color / 255.0f;
            var fa = alpha / 255.0f;
            var fr = (int)(255.0f * fc * fa);
            if (fr < 0)
            {
                fr = 0;
            }
            if (fr > 255)
            {
                fr = 255;
            }
            return (byte)fr;
        }

        public static void PremultiplyAlpha(Texture2D texture)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);

            for (int i = 0; i < data.Length; ++i)
            {
                byte a = data[i].A;

                data[i].R = ApplyAlpha(data[i].R, a);
                data[i].G = ApplyAlpha(data[i].G, a);
                data[i].B = ApplyAlpha(data[i].B, a);
            }

            texture.SetData(data);
        }

        ////move to graphics utils
        //public void CameraCircle(Vector2 position, float rad, Color color)
        //{
        //    var sprite = Sprite.Get("circle64");  //change
        //    SpriteBatch.Draw(sprite.Texture, GetScreenPos(position), null, color, 0, new Vector2(sprite.Width / 2, sprite.Height / 2)
        //        , Zoom * rad / 32f, SpriteEffects.None, 0);
        //}


        public static Texture2D SmartTint(Texture2D texture, Color targetColor, float intencity = 0.4f)
        {

            float h, s, v;
            float refH;
            RgbToHsv(targetColor.R, targetColor.G, targetColor.B, out refH, out s, out v);
            Canvas canvas = new Canvas(texture);
            Canvas targetCanvas = new Canvas(texture.Width, texture.Height, texture.GraphicsDevice);
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Color pixel = canvas.GetPixel(x, y);
                    byte alpha = pixel.A;
                    
                    RgbToHsv(pixel.R, pixel.G, pixel.B, out h, out s, out v);
                    float value = MathHelper.Clamp(s * v + 1- intencity, 0, 1); ;
                    pixel = new Color(pixel.ToVector3() * value + targetColor.ToVector3() * (1 - value));
                    float newV;
                    RgbToHsv(pixel.R, pixel.G, pixel.B, out h, out s, out newV);                    
                    pixel = GraphicsUtils.HsvToRgb(h, s, v);
                    pixel.A = alpha;

                    targetCanvas.SetPixel(x, y, pixel);
                }
            }
            targetCanvas.SetData();
            return targetCanvas.GetTexture();
        }

        //public static Extensions 

        //public static void Draw(this SpriteBatch sb, )
        //{
            
        //}

    }
}
