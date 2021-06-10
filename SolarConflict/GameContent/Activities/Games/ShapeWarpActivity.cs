//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using XnaUtils;
//using XnaUtils.Framework.Graphics;

//namespace SolarConflict.GameContent.Activities.Games
//{
//    class ShapeWarpActivity : Activity
//    {
//        int counter = 0;
//        float rotation = MathHelper.PiOver4;

//        public override void Update(InputState inputState)
//        {
//            if (inputState.IsKeyPressed(Keys.Escape))
//                ActivityManager.Inst.Back();
//            if (inputState.IsKeyPressed(Keys.Space))
//                counter++;
//            if (inputState.IsKeyPressed(Keys.Back) && counter > 0)
//                counter--;
//            if (inputState.IsKeyDown(Keys.Up))
//                rotation += 0.01f;
//            if (inputState.IsKeyDown(Keys.Down))
//                rotation -= 0.01f;
//        }

//        private void DrawPoly(Vector2 pos, int shapeType, float size, int start = 0)
//        {
            
//            for (int n = start; n < shapeType; n++)
//            {
//                //float rotation = MathHelper.PiOver4;
//                float angle = MathHelper.TwoPi / shapeType * n;
//                Vector2 point1 = pos + new Vector2((float)Math.Cos(angle + rotation), (float)Math.Sin(angle + rotation)) * size;
//                angle = MathHelper.TwoPi / shapeType * (n + 1);
//                Vector2 point2 = pos + new Vector2((float)Math.Cos(angle + rotation), (float)Math.Sin(angle + rotation)) * size;
//                GraphicsUtils.Line(ActivityManager.SpriteBatch, point1, point2, Color.Azure, 0.5f, GraphicsUtils.DefaultSetPixel);
//            }
//        }

//        private void DrawShape(Vector2 pos, int shapeType)
//        {

//            float size = 200;
//            if (shapeType == 0)
//            {
//                DrawPoly(pos, 1000, size);
//            }

//            if (shapeType == 1)
//            {
//                DrawPoly(pos, 2, size);
//            }

//            if(shapeType == 2)
//            {
//                DrawPoly(pos, 3, size, 1);
//            }

//            if (shapeType >= 3)
//            {
//                DrawPoly(pos, shapeType, size);
//            }
//        }

//        public override void Draw(SpriteBatch sb)
//        {
//            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive);            
//            DrawShape(ActivityManager.ScreenCenter, counter);

//            sb.DrawString(Game1.menuFont, counter.ToString(), ActivityManager.ScreenCenter - Vector2.UnitY * 300, Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
//                //ActivityManager.ScreenCenter - Vector2.UnitY * 300, Color.White);
//            sb.End();
//        }

//        public static Activity ActivityProvider(string parameters)
//        {
//            return new ShapeWarpActivity();
//        }
//    }
//}
