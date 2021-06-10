//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace PaintPlay
//{
//    class RealSun:RealObject
//    {
        

//        public RealSun()
//        {
//            rotation = 0;
//            texture = MyGraphics.GetTexture("sun");
//            this.textureScale = 0.3f;
//        }

//        private Color GetColor(Vector2 pos)
//        {
//            float diff = Math.Abs((pos.X-MyGraphics.screenRect.Width/2) /(float)(MyGraphics.screenRect.Width/2));
//            int i = (int)((1-diff) * 255);
//            byte r = (byte)Math.Min(i * 4+50, 255);
//            byte g = (byte)Math.Max(Math.Min((i - 64) * 4, 255), 50);
//            byte b = (byte)Math.Max(Math.Min((i - 64 * 2) * 4, 255), 50);
//            return new Color(r, g, b);
//        }

//        public override void Update(GardenLogic gLogic)
//        {
//            base.Update(gLogic);
//            position = Game1.objPos;
//            gLogic.dayTimeColor = GetColor(this.position);
//            rotation += 0.01f;
//            if (life < 1)
//            {
//                gLogic.dayTimeColor = Color.White; 
//            }

//            int x = (int)(Game1.objPos.X * (gLogic.mcga.Width / (float)MyGraphics.screenRect.Width));
//            int y = (int)(Game1.objPos.Y * (gLogic.mcga.Height / (float)MyGraphics.screenRect.Height));
//            GPixel pixel = new GPixel();
//            pixel.type = PixelType.Fire;
//            pixel.value = 500;

//           // int r = MyMath.rand.Next(50);
//            //pixel.color = new Color((byte)(r + 200), (byte)(r + 200), (byte)(r + 200));

           
                
//            GardenHelper.Line(x - 6, y - 6, x+ 6, y+6, pixel, gLogic.grid);
//            GardenHelper.Line(x - 6, y +  6, x - 6, y + 6, pixel, gLogic.grid);
            

//        }

//        public override void Draw()
//        {
//            base.Draw();
//         //   MyGraphics.sb.DrawString(MyGraphics.font, position.ToString(),new Vector2(300, 300), Color.Red);
//        }
//    }
//}
