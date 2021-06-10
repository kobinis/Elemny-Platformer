//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace PaintPlay
//{
//    class RealMoon : RealObject
//    {
//        Vector2 prevPos = new Vector2();

//        public RealMoon()
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
//         //   gLogic.dayTimeColor = GetColor(this.position);
          
//            int x = (int)(position.X * (gLogic.mcga.Width / (float)MyGraphics.screenRect.Width));
//            int y = (int)(position.Y * (gLogic.mcga.Height / (float)MyGraphics.screenRect.Height));
//            GPixel pixel = new GPixel();
//            pixel.type = PixelType.Plant;
//            int r = MyMath.rand.Next(50);
//            pixel.color = Color.White;

//            if((prevPos-position).LengthSquared() > 20)
//            {
//            for (int i = x - 1; i < x + 1; i++)
//            {
//                pixel.param1 = MyMath.rand.Next(3) - 1;
//                pixel.param2 = MyMath.rand.Next(3) - 1;
//                if (MyMath.rand.Next(4) == 0)
//                    GardenHelper.Line(i - 1, y, i + 1, y, pixel, gLogic.grid);
//            }
//             }

//        }

//        public override void Draw()
//        {
//            base.Draw();
//        }
//    }
//}
