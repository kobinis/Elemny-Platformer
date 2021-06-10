//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace PaintPlay
//{
//    class SnowCloud : RealObject
//    {


//        public SnowCloud()
//        {
//            rotation = 0;
//            texture = MyGraphics.GetTexture("snowcloud");
//            this.textureScale = 0.1f;
//        }



//        public override void Update(GardenLogic gLogic)
//        {
//            base.Update(gLogic);
//            //this.rotation = this.contact.Orientation;
//            int x = (int)(position.X * (gLogic.mcga.Width / (float)MyGraphics.screenRect.Width));
//            int y = (int)(position.Y * (gLogic.mcga.Height / (float)MyGraphics.screenRect.Height));
//            GPixel pixel = new GPixel();
//            pixel.type = PixelType.Snow;
//            int r = MyMath.rand.Next(50);
//            pixel.color = new Color((byte)(r + 200), (byte)(r + 200), (byte)(r + 200));

//            for (int i = x - 15; i < x + 15; i++)
//            {
//                pixel.param1 = MyMath.rand.Next(3) - 1;
//                pixel.param2 = MyMath.rand.Next(3) - 1;
//                if (MyMath.rand.Next(3) == 0)
//                    GardenHelper.Line(i - 1, y, i + 1, y, pixel, gLogic.grid);
//            }

//        }

//        public override void Draw()
//        {
//            base.Draw();
//        }


//    }
//}
