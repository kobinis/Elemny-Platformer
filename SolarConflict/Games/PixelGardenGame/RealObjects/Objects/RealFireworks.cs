//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace PaintPlay
//{
//    class RealFireworks : RealObject
//    {
//        Vector2 prevPos = new Vector2();
//        int delay = 0;

//        public RealFireworks()
//        {
//            rotation = 0;
//            texture = MyGraphics.GetTexture("flower1");
//            this.textureScale = 0.3f;
//        }

//        private Color GetColor(Vector2 pos)
//        {
//            float diff = Math.Abs((pos.X - MyGraphics.screenRect.Width / 2) / (float)(MyGraphics.screenRect.Width / 2));
//            int i = (int)((1 - diff) * 255);
//            byte r = (byte)Math.Min(i * 4 + 50, 220);
//            byte g = (byte)Math.Max(Math.Min((i - 64) * 4, 200), 50);
//            byte b = (byte)Math.Max(Math.Min((i - 64 * 2) * 4, 200), 50);
//            return new Color(b, g, r);
//        }

//        public override void Update(GardenLogic gLogic)
//        {
//            prevPos = position;
//            base.Update(gLogic);
//            //   gLogic.dayTimeColor = GetColor(this.position);

//            int x = (int)(position.X * (gLogic.mcga.Width / (float)MyGraphics.screenRect.Width));
//            int y = (int)(position.Y * (gLogic.mcga.Height / (float)MyGraphics.screenRect.Height));
//            GPixel pixel = new GPixel();
//            pixel.type = PixelType.MusicFireworks;
//            pixel.value = 1; //from the colors
//            pixel.param1 = 150;
//            pixel.param2 = 0;
           
            
//            int r = MyMath.rand.Next(50);
//            pixel.color = Color.Red;

//            int rady = 1;
//            int radx = 1;
//            delay--;
//            if (delay <= 0)
//            {
//                delay = 20;
//                for (int j = -rady; j <= +rady; j++)
//                {
//                    for (int i = -radx; i <= rady; i++)
//                    {
//                        // pixel.param1 = 150 + MyMath.rand.Next(5) * 20;
//                        //pixel.color = Painter.palette[MyMath.rand.Next(600) % Painter.palette.Length];
//                        int xx = i * 65 + x + j * 5;
//                        int yy = j * 20 + y;
//                        if (MyMath.rand.Next(4) == 0)
//                            GardenHelper.Line(xx - 1, yy, xx + 1, yy, pixel, gLogic.grid);
//                    }
//                }
//            }

//        }

//        public override void Draw()
//        {
//           // base.Draw();
//        }
//    }
//}
