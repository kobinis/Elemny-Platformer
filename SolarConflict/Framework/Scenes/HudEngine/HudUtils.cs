using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using XnaUtils.Graphics;
using XnaUtils;

namespace SolarConflict.Framework.Scenes.HudEngine
{
    class HudUtils
    {
        public static Sprite barTexture = Sprite.Get("greybar"); //dangerous
        public static Sprite hudTexture = Sprite.Get("statBars");
        public static Sprite enemyBarsTexture = Sprite.Get("enemyBars");


        /*public static void MeterDisplay(SpriteBatch sb, float value, Vector2 position, float scale, Color color)
        {
            sb.Draw(barTexture,)
        }*/

        public static void DrawArrow(SpriteBatch sb, Sprite sprite, Vector2 position) //pass Color, size
        {
            float offset = (float)(Math.Cos((Game1.time / 10f)) * 0.5 + 0.5);
            int size = 30;
            position += Vector2.UnitY * (size + offset * size / 2f);
            Rectangle rect = new Rectangle((int)position.X - size / 2, (int)position.Y - size / 2, size, size);
            sb.Draw(sprite.Texture, rect, Color.Yellow);
        }

        public static void MeterDisplay(SpriteBatch sb, float value, Rectangle rectangle, Color color)
        {

            value = MathHelper.Clamp(value, 0, 1);

            Color backColor = new Color(color.ToVector3() * 0.2f);
            backColor.A = color.A;

            Rectangle sourceRectangle = new Rectangle(0,0, (int)(value * barTexture.Width) , barTexture.Height);
            sb.Draw(barTexture, rectangle , backColor);

            Rectangle rect = rectangle;
            rect.Width = (int)(value * rectangle.Width);
            sb.Draw(barTexture, rect, sourceRectangle, color);            
        }

        public static void HpSegmentDisplay(SpriteBatch sb, Rectangle targetRectangle, Rectangle sourceRectangle, Color color)
        {
            sb.Draw(enemyBarsTexture, targetRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
            
        }

        public static void HpSegmentDisplay(SpriteBatch sb, Vector2 position, Rectangle sourceRectangle, Color color, float zoomFactor)
        {
            sb.Draw(enemyBarsTexture, position, sourceRectangle, color, 0, Vector2.Zero, zoomFactor, SpriteEffects.None, 1.0f);

        }

        public static void BasicMeterDisplay(SpriteBatch sb, Rectangle targetRectangle, Rectangle sourceRectangle, Color color)
        {
            sb.Draw(enemyBarsTexture, targetRectangle, sourceRectangle, color, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
        }

        public static void MeterDisplayV2(SpriteBatch sb, Rectangle targetRectangle, Rectangle sourceRectangle, Rectangle fillTarRectangle, Rectangle fillRectangle)
        {
            //Underlay
            sb.Draw(hudTexture, targetRectangle, sourceRectangle, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
            //Fill
            sb.Draw(hudTexture, fillTarRectangle, fillRectangle, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
        }

        public static void DrawArrow(Camera camera, Vector2 worldPosition, float targetSize, Sprite texture, Color color, float posMult) //TODO: change to static
        {
            Vector2 diffVec = worldPosition - camera.Position;
            float distance = (int)(diffVec.Length() - targetSize);
            Vector2 postionOnScreen = camera.GetScreenPos(worldPosition);
           
            float angle = (float)Math.Atan2(diffVec.Y, diffVec.X);

            Vector2 drawPostion = Vector2.Zero;


            Vector2 drawPostion1 = camera.GetScreenPos(camera.Position + diffVec.Normalized() * (distance*0.8f-20)) - ActivityManager.ScreenCenter; ;

            Vector2 drawPostion2 = FMath.RectangleIntersectionPoint(diffVec, ActivityManager.ScreenCenter - Vector2.One * 50);

         //   float arrowMovment = 0.95f -MathHelper.Clamp((float)Math.Cos(ActivityManager.Inst.GameTime.TotalGameTime.TotalMilliseconds * 0.001f),0, 1)*0.5f;
            drawPostion = FMath.MinMagVector(drawPostion1, drawPostion2 * posMult)  + ActivityManager.ScreenCenter;

            string arrowText = ((int)(distance * Consts.PixelsToUinits)).ToString();
            Vector2 textSize = Game1.orbitron12.MeasureString(arrowText);
            //Push text color towards white
            Color textColor = Color.Lerp(color, Color.White, 0.7f);
            textColor.A = color.A;  
            camera.SpriteBatch.Draw(texture, drawPostion, null, color, angle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
            camera.SpriteBatch.DrawString(Game1.orbitron12, arrowText, drawPostion - textSize * 0.5f + Vector2.One*1, new Color((byte)0, (byte)0, (byte)0, color.A));
            camera.SpriteBatch.DrawString(Game1.orbitron12, arrowText, drawPostion - textSize * 0.5f - Vector2.One*1, new Color((byte)0, (byte)0, (byte)0, color.A));
            camera.SpriteBatch.DrawString(Game1.orbitron12, arrowText, drawPostion - textSize * 0.5f, textColor);
            

        }


    }
}
